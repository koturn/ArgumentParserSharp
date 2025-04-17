SOLUTION_NAME = ArgumentParserSharp
SOLUTION_FILE = $(SOLUTION_NAME).sln
MAIN_PROJECT_FILE = $(SOLUTION_NAME)\$(SOLUTION_NAME).csproj
PROJECT_DIRS = $(SOLUTION_NAME)
ARTIFACTS_BASEDIR = Artifacts
ARTIFACTS_SUBDIR_BASENAME = $(SOLUTION_NAME)
ARTIFACTS_BASENAME = $(SOLUTION_NAME)
BUILD_CONFIG = Release
BUILD_PLATFORM = "Any CPU"
TARGET_NETSTD20 = netstandard2.0
TARGET_NET6 = net6.0
TARGET_NET8 = net8.0
TARGET_NET9 = net9.0
RM = del /F /Q


all: build

build:
	dotnet build -c $(BUILD_CONFIG) $(SOLUTION_FILE)

restore:
	dotnet restore $(SOLUTION_FILE)

deploy: deploy-$(TARGET_NETSTD20) deploy-$(TARGET_NET6) deploy-$(TARGET_NET8) deploy-$(TARGET_NET9)

deploy-$(TARGET_NETSTD20):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NETSTD20) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NETSTD20) \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NETSTD20)\*.pdb \
		$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NETSTD20)\*.xml \
		$(ARTIFACTS_BASENAME)-$(TARGET_NETSTD20).zip 2>NUL
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NETSTD20) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NETSTD20).zip
	cd $(MAKEDIR)

deploy-$(TARGET_NET6):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NET6) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET6) \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET6)\*.pdb \
		$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET6)\*.xml \
		$(ARTIFACTS_BASENAME)-$(TARGET_NET6).zip 2>NUL
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET6) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NET6).zip
	cd $(MAKEDIR)

deploy-$(TARGET_NET8):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NET8) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET8) \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET8)\*.pdb \
		$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET8)\*.xml \
		$(ARTIFACTS_BASENAME)-$(TARGET_NET8).zip 2>NUL
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET8) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NET8).zip
	cd $(MAKEDIR)

deploy-$(TARGET_NET9):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NET9) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9) \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)\*.pdb \
		$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)\*.xml \
		$(ARTIFACTS_BASENAME)-$(TARGET_NET9).zip 2>NUL
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NET9).zip
	cd $(MAKEDIR)

clean:
	-for %%d in ( $(PROJECT_DIRS) ) do @( \
		@$(RMDIR) %%d\bin %%d\obj 2>NUL \
	)

distclean: clean
	$(RM) $(ARTIFACTS_BASEDIR) $(ARTIFACTS_BASENAME)-*.zip 2>NUL
