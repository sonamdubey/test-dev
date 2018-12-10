IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertInquiryDistribution]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertInquiryDistribution]
GO

	
CREATE PROCEDURE [InsertInquiryDistribution] 
	@CustomerId			NUMERIC,
	@InquiryId			NUMERIC,
	@InquiryTypeId			INT,
	@ServiceProviderId		NUMERIC,
	@EntryDate			DATETIME,
	@UserId			NUMERIC, 	-- Who has forwarded this Inquiry.
	@NextCallDate			DATETIME	
		
AS
	DECLARE @FollowupID		NUMERIC

BEGIN
	SET @FollowupId = 0
	SELECT @FollowupId=Id 
	FROM InquiryDistribution 
	WHERE ServiceProviderID=@ServiceProviderId AND InquiryId=@InquiryId AND InquiryTypeId=@InquiryTypeId

	IF @FollowupId <> 0
	BEGIN
		INSERT INTO FollowupDetails(FollowupID,FollowupDescription,FollowedbyID,
			FollowupDate,nextFollowupDate,StatusId)
		VALUES(@FollowupId, 'Inquiry forwarded again on ' + CONVERT(VARCHAR,@EntryDate,101), @UserId,
			@EntryDate, @NextCallDate,1)
	END
	ELSE
	BEGIN
		INSERT INTO InquiryDistribution(CustomerId, InquiryTypeId, InquiryId, ServiceProviderId, EntryDate)
		VALUES(@CustomerId, @InquiryTypeId, @InquiryId, @ServiceProviderId, @EntryDate)	

		SET @FollowupId = SCOPE_IDENTITY()		

		INSERT INTO FollowupDetails(FollowupID,FollowupDescription,FollowedbyID,
			FollowupDate,nextFollowupDate,StatusId)
		VALUES(@FollowupId, 'Inquiry forwarded on ' + CONVERT(VARCHAR,@EntryDate,101), @UserId,
			@EntryDate, @NextCallDate,1)
	END
	
END
