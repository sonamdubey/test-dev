IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFreeFuelOfferCandidates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFreeFuelOfferCandidates]
GO
	
-- =============================================
-- Author:		PRACHI PHALAK
-- Create date: 30 SEPT,2015
-- Description:	Inserting Candidate's record into FreeFuelOfferCandidates table for Fuel Offer.
-- =============================================
CREATE PROCEDURE [dbo].[InsertFreeFuelOfferCandidates] 
	-- Add the parameters for the stored procedure here
	@Name VARCHAR(80),
	@Email VARCHAR(100),
	@Mobile VARCHAR(15),
	@CWGTDPolicyNo VARCHAR(50),
	@ReasonBuyingCWGTD VARCHAR(160),
	@EntryDateTime DATETIME,
	--@Iswinner BIT = 0,
	--@WinningDate DATETIME = NULL,
	@Response INT OUTPUT
AS
BEGIN
		BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     -- Insert statements for procedure here
	INSERT INTO FreeFuelOfferCandidates
	(Name,Email,Mobile,CWGTDPolicyNo,ReasonBuyingCWGTD,EntryDateTime,Iswinner,WinningDate)
	VALUES(@Name,@Email,@Mobile,@CWGTDPolicyNo,@ReasonBuyingCWGTD,@EntryDateTime,0,Null)
	
	SET @Response = SCOPE_IDENTITY()
	END TRY
	BEGIN CATCH
		 INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Used Car Free Fuel Offer',
									        'dbo.InsertFreeFuelOfferCandidates',
											 ERROR_MESSAGE(),
											 'FreeFuelOfferCandidates',
											 'MobileNo:' + @Mobile,
											 GETDATE()
                                            )

		END CATCH ;
END
		

