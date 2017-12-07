NAME = wxnet.dll
VERSION = $(getver)
TARGET = library

ROOT_SOURCE_DIR = src
getSources = $(shell find $(ROOT_SOURCE_DIR) -name "*.cs")
getVer = $(shell grep -Eo "[0-9]+\.[0-9]\.[0-9]+" $(ROOT_SOURCE_DIR)/wx/AssemblyInfo.cs)

SRC = $(getSources)

REFS = System.Data.dll System.Drawing.dll 
REFS_FLAG = $(addprefix -r:, $(REFS))
#PKG_FLAG +=-pkg:gtk-sharp-2.0
#RES_OPT = -res:resources/ml_white32.ico,ml.ico -res:resources/ml.png,ml.png

ZIP = $(BIN)/$(NAME)
ZIP_SRC = $(ZIP) $(SRC) README.md CHANGELOG.md makefile $(SRC_TEST)

########
# Test #
########
TEST_SOURCE_DIR = tests
SRC_TEST = $(filter-out $(ROOT_SOURCE_DIR)/App.cs, $(SRC))
SRC_TEST += $(wildcard $(TEST_SOURCE_DIR)/*.cs)

REFS_TEST = $(REFS)
REFS_TEST += nunitlite.dll
REFS_TEST += nunit.framework.dll
REFS_TEST += NSubstitute.dll
#REFS_TEST += Ploeh.AutoFixture.dll
REFS_FLAG_TEST = $(addprefix -r:, $(REFS_TEST))
PKG_FLAG_TEST = $(PKG_FLAG)

###############
# Common part #
###############
BIN = bin
CSC = mcs

BASE_NAME = $(basename $(NAME))
#CSCFLAGS += -debug
#ISO-1, ISO-2, 3, 4, 5, Default or Experimental
#CSCFLAGS += -langversion:3
#anycpu|anycpu32bitpreferred|arm|x86|x64|itanium
#CSCFLAGS += -platform:x86

# vedere /usr/lib/mono
#CSCFLAGS += -sdk:2|4|4.5
#CSCFLAGS += -sdk:2
CSCFLAGS += -nologo
CSCFLAGS += -target:$(TARGET)
CSCFLAGS += -lib:$(BIN)
CSCFLAGS += $(RES_OPT)

NAME_TEST = test-runner
CSCFLAGS_TEST += -debug -nologo -target:exe
CSCFLAGS_TEST += -lib:$(BIN)

NUNIT_OPT =--noheader --noresult

PUBLISH_DIR = $(CS_DIR)/lib/Microline/$(BASE_NAME)/$(VERSION)
ZIP_PREFIX = $(BASE_NAME)-$(VERSION)

.PHONY: all clean clobber test testv ver var pkgall pkg pkgtar pkgsrc publish init

DEFAULT: all
all: builddir $(BIN)/$(NAME)

WHERE += $(if $(W), --where "$(W)")

## make test W=test_name T=option
test: builddir $(BIN)/$(NAME_TEST)
	mono $(BIN)/$(NAME_TEST) $(NUNIT_OPT) $(T) $(WHERE)

testv: builddir $(BIN)/$(NAME_TEST)
	mono $(BIN)/$(NAME_TEST) $(NUNIT_OPT) -v $(T) $(WHERE)

builddir:
	@mkdir -p $(BIN)

$(BIN)/$(NAME): $(SRC) | builddir
	$(CSC) $(CSCFLAGS) $(REFS_FLAG) $(PKG_FLAG) -out:$@ $^

$(BIN)/$(NAME_TEST): $(SRC_TEST) | builddir
	$(CSC) $(CSCFLAGS_TEST) $(REFS_FLAG_TEST) $(PKG_FLAG_TEST) -out:$@ $^

pkgdir:
	@mkdir -p pkg

pkgall: pkg pkgtar pkgsrc

pkg: pkgdir | pkg/$(ZIP_PREFIX).zip

pkg/$(ZIP_PREFIX).zip: $(ZIP)
	zip $@ $(ZIP)

pkgtar: pkgdir | pkg/$(ZIP_PREFIX).tar.bz2

pkg/$(ZIP_PREFIX).tar.bz2: $(ZIP)
	tar -jcf $@ $^

pkgsrc: pkgdir | pkg/$(ZIP_PREFIX)-src.tar.bz2

pkg/$(ZIP_PREFIX)-src.tar.bz2: $(ZIP_SRC)
	tar -jcf $@ $^

changelog: CHANGELOG.txt

CHANGELOG.txt: CHANGELOG.md
	pandoc -f markdown_github -t plain $^ > $@

init:
	@mkdir -p $(TEST_SOURCE_DIR)
	@mkdir -p $(ROOT_SOURCE_DIR)/$(BASE_NAME)
	@touch README.md
	@touch CHANGELOG.md

publishdir:
	@mkdir -p $(PUBLISH_DIR)

publish: publishdir
	cp -u --verbose --backup=t --preserve=all $(BIN)/$(NAME) $(PUBLISH_DIR)

tags: $(SRC)
	ctags $^

ver:
	@echo $(VERSION)

clean:
	-rm -f $(BIN)/$(NAME)
	-rm -f $(BIN)/$(NAME_TEST)
	-rm -f $(BIN)/*.mdb

clobber: clean
	-rm -Rf $(BIN)/*.dll

var:
	@echo "General"
	@echo "--------------------"
	@echo "NAME            : " $(NAME)
	@echo "BIN_NAME        : " $(BIN_NAME)
	@echo "ROOT_SOURCE_DIR : " $(ROOT_SOURCE_DIR)
	@echo "CSCFLAGS        : " $(CSCFLAGS)
	@echo "REFS            : " $(REFS)
	@echo "REFS_FLAG       : " $(REFS_FLAG)
	@echo "VERSION         : " $(VERSION)
	@echo 
	@echo "Test"
	@echo "--------------------"
	@echo "REFS_TEST     : " $(REFS_TEST)
	@echo "CSCFLAGS_TEST : " $(CSCFLAGS_TEST)

varsrc:
	@echo 
	@echo "Zip"
	@echo "--------------------"
	@echo "ZIP_BIN    : " $(ZIP_BIN)
	@echo "ZIP_PREFIX :" $(ZIP_PREFIX)
	@echo 
	@echo "Zip source"
	@echo "--------------------"
	@echo $(ZIP_SRC)
	@echo 
	@echo "Source"
	@echo "--------------------"
	@echo $(SRC)
	@echo 
	@echo "Source test"
	@echo "--------------------"
	@echo "SRC_TEST : " $(SRC_TEST)
	@echo 

#include i18n.makefile
