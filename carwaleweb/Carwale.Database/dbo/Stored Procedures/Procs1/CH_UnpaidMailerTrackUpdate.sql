IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_UnpaidMailerTrackUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_UnpaidMailerTrackUpdate]
GO

	-- Author:		Dilip V. 
-- Create date: 05-Sept-2012
-- Description:	Get @LoginId,@Password from CH_UnpaidMailerTrack
				
CREATE PROCEDURE [dbo].[CH_UnpaidMailerTrackUpdate]	
	@Id			NUMERIC(18,0),
	@InquiryId	NUMERIC(18,0) OUTPUT,
	@CustomerId	NUMERIC(18,0) OUTPUT,
	@LoginId	VARCHAR(100) OUTPUT,
	@Password	VARCHAR(20) OUTPUT,
	@LandingUrl VARCHAR(250) OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	
	IF EXISTS(
	SELECT InquiryId FROM CH_UnpaidMailerTrack WITH(NOLOCK)	WHERE Id = @Id)
	BEGIN
		SELECT @InquiryId = CUMT.InquiryId, @CustomerId = CUMT.CustomerId, @LoginId = CC.email, 
				@Password = CC.password, @LandingUrl = MT.LandingUrl
		FROM CH_UnpaidMailerTrack CUMT WITH(NOLOCK)
			JOIN Customers CC WITH(NOLOCK) ON CC.Id = CUMT.CustomerId
			LEFT JOIN MailerTypes MT WITH(NOLOCK) ON MT.MailerTypesId = CUMT.MailerTypesId
		WHERE CUMT.Id = @Id
		
		UPDATE CH_UnpaidMailerTrack SET IsClicked = 1 , ClickDate = GETDATE() WHERE Id = @Id
	
	END
END



