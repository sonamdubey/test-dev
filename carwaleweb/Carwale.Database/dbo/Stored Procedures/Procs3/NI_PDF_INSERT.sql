IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NI_PDF_INSERT]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NI_PDF_INSERT]
GO

	

CREATE PROCEDURE [dbo].[NI_PDF_INSERT]
@FirstName VARCHAR(50),
@LastName VARCHAR(50),
@Email VARCHAR(50),
@ContactNo VARCHAR(50),
@CityId NUMERIC(18,0),
@VersionId NUMERIC(18,0),
@DateOfPurchase DATETIME = '01/01/1900',
@KilometersDone NUMERIC(18,0) = 0
AS
BEGIN
	INSERT INTO NI_PDF
	(FirstName,LastName,Email,ContactNo,CityId,VersionId,DateOfPurchase,KilometersDone)
	VALUES
	(@FirstName,@LastName,@Email,@ContactNo,@CityId,@VersionId,@DateOfPurchase,@KilometersDone)
END
