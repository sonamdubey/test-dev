IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_ChatRecords_Repository]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_ChatRecords_Repository]
GO

	-- =============================================
-- Author:		Purohith Guguloth
-- Create date: 28-09-2015
-- Description:	This Inserts Chat Records into the table ChatRepository
-- =============================================
CREATE PROCEDURE [dbo].[Classified_ChatRecords_Repository]
	-- Add the parameters for the stored procedure here
	@SenderQuickBloxId VARCHAR(20), 
	@DealerQuickBloxId VARCHAR(20),
	@ChatId VARCHAR(50),
	@DeviceId VARCHAR(100),
	@InquiryId INT,
	@SellerType BIT,
	@SenderName VARCHAR(50) = NULL,
	@SenderMobile VARCHAR(50) = NULL,
	@SenderEmail VARCHAR(50) = NULL,
	@CreatedOn DATETIME,
	@ChatRepoId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    IF @ChatId IS NOT NULL AND @ChatId <> ''
    BEGIN
		INSERT INTO ChatRepository
		(
			SenderQuickBloxId,
			DealerQuickBloxId,
			ChatId,
			DeviceId,
			InquiryId,
			SellerType,
			SenderName,
			SenderMobile,
			SenderEmail,
			CreatedOn
		)
		VALUES
		(
			@SenderQuickBloxId, 
			@DealerQuickBloxId,
			@ChatId,
			@DeviceId,
			@InquiryId,
			@SellerType,
			@SenderName,
			@SenderMobile,
			@SenderEmail,
			@CreatedOn
		)
		SET @ChatRepoId = SCOPE_IDENTITY()
	END	
END

