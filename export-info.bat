@echo off
svn export . exported
tree exported /f > Resources\FLocal\static\info\tree.txt
rmdir /s /q exported
svn log -r HEAD:0 -v > Resources\FLocal\static\info\svnlog.txt
