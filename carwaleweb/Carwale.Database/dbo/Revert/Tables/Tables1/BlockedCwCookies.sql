IF EXISTS (
 		SELECT *
 		FROM sysobjects
 		WHERE NAME = 'BlockedCwCookies'
 			AND xtype = 'U'
 		)
 BEGIN
 	DROP TABLE BlockedCwCookies
 END
 GO