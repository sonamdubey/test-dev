EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'carwale';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'cwreadonly';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'cwuser1';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'cwuser2';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'cwuser3';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'cwuser4';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'bwuser1';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'AEPL\Read_Only_DB';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'cwbwreadonly';

