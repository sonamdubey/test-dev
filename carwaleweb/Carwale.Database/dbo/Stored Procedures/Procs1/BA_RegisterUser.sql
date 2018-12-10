IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_RegisterUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_RegisterUser]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_RegisterUser]
@BrokerMobile VARCHAR(50),
@BrokerName VARCHAR(50),
@JoiningDate DATETIME 


AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Status INT = -1  --Some Error ocured

	---Check ||  Mobile Number Already Exists. 
	IF (SELECT Count(BR.ID) FROM BA_RegisterBroker AS BR WHERE BR.BrokerMobile = @BrokerMobile) = 0
	BEGIN
					INSERT INTO [dbo].[BA_RegisterBroker]
						   ([BrokerMobile]
						   ,[BrokerName]
						   ,[IsVerified]
						   ,[IsActive]
						   ,DownloadDate)
					 VALUES
							(@BrokerMobile
						   ,@BrokerName
						   ,0
						   ,0
						   ,@JoiningDate)

	
	SET @Status = @@ROWCOUNT  ---If Insert is Successful
	
	END
	ELSE
	SET @Status = 0 ----Mobile already Exist 
	
	SELECT @Status AS Status

END
