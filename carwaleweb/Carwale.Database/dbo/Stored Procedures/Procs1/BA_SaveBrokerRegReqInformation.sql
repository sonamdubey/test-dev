IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_SaveBrokerRegReqInformation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_SaveBrokerRegReqInformation]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 28-may-14
-- Description:	Save the Broker Registration Request by OPR
-- =============================================
CREATE PROCEDURE [dbo].[BA_SaveBrokerRegReqInformation]
	@Mobile VARCHAR(50),
	@Name VARCHAR(100),
	@Email VARCHAR(100),
	@CityId INT,
	@StateId INT,
	@DateOfJoining DATETIME,
	@DateOfActivation DATETIME,
	@Comments VARCHAR(1000)
AS
BEGIN
DECLARE @ResultMess VARCHAR(100) = NULL 
	SET NOCOUNT ON;
BEGIN TRY
--IF (SELECT BR.IsVerified FROM  BA_RegisterBroker AS BR WHERE BR.BrokerMobile = @Mobile) <> 1
--BEGIN
	UPDATE  [dbo].[BA_RegisterBroker] SET
            [BrokerName] = @Name
           ,[Email] = @Email
           ,[CityId] = @CityId
		   ,[StateId] = @StateId
           ,[DownloadDate] = @DateOfJoining
           ,[DateofActivation] = @DateOfActivation
           ,[EndOfActivation] = NULL
           ,[ActiveDays] = NULL
           ,[ContactHours] = NULL
           ,[Comments] = @Comments
           ,[IsVerified] = 1
          -- ,[IsActive] = 0
WHERE   [BrokerMobile] = @Mobile

IF @@RowCount > 0
SET  @ResultMess = 'Broker has been Verified. and Data has been saved.'
ELSE
SET  @ResultMess = 'Error while Saving Broker Data.'
--END
--ELSE
--BEGIN
-- SET  @ResultMess = 'Broker already has been Verified.' --Unique key Viol
--END

 END TRY
 BEGIN CATCH 
 if (ERROR_NUMBER()= 2627)
BEGIN
    SET  @ResultMess = 'Mobile Number Already Exist.' --Unique key Viol
END
 END CATCH
 
 SELECT @ResultMess AS Result 
END
