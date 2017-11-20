@echo off
::setlocal enabledelayedexpansion
title Copy_Sitemaps

::SET src_content_path=C:\work\bikewaleweb\BikeWale.UI\sitemaps
SET src_content_path=%1
::ECHO %src_content_path%

::SET dest_content_path=e:\sitemaps
SET dest_content_path=%2
::ECHO %dest_content_path%

:: Array (space separated is array) for computer names 10.10.3.30 10.10.4.30 10.10.3.31
SET computer_name=(10.10.3.110)
SET user_name=AEPL\ashish.kamble
SET user_password=Pass@123


echo Started copying files to other server >> log.txt

for %%x in %computer_name% do ("C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\msdeploy" -verb:sync -source:contentPath=%src_content_path% -dest:contentPath=%dest_content_path%,computerName=%%x,username=%user_name%,password=%user_password% -AllowUntrusted -enableRule:DoNotDeleteRule)

::"C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\msdeploy" -verb:sync -source:contentPath=%src_content_path% -dest:contentPath=%dest_content_path%,computerName=%computer_name%,username=%user_name%,password=%user_password% -AllowUntrusted -enableRule:DoNotDeleteRule

echo Finished copying files to other server >> log.txt

::pause