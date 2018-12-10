IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertRecordMissCallVerification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertRecordMissCallVerification]
GO
	
-- =============================================
-- Author:		Akansha
-- Create date: 19.06.2013
-- Description:	Record For ZipDial Service Exec InsertRecordForZipDial '9967821412', 'DC-D4554-','D45554',2
-- Modified By: Akansha increased the varchar size of clientId and ReturnedClientId
-- Modified By: Amit, added source column 10/31/2013
-- modified By Rakesh Yadav on 24 aug 2016, removed insertion and other code specific to used car,
-- Note: this SP is renamed from InsertRecordForZipDial to InsertRecordMissCallVerification
-- =============================================
CREATE PROCEDURE [dbo].[InsertRecordMissCallVerification] 
	@MobileNumber VARCHAR(15)
	,@Email VARCHAR(50)
	,@Source SMALLINT = NULL
AS
BEGIN
	INSERT INTO ZipDial_Transactions (
		MobileNumber
		,CreatedTime
		,Email
		,Source -- Modified By: Amit, added source column 10/31/2013
		)
	VALUES (
		@MobileNumber
		,GetDate()
		,@Email
		,@Source  --Modified By: Amit, added source column 10/31/2013
		)	
END

