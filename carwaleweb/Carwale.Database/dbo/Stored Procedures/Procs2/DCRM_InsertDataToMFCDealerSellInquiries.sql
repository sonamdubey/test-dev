IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_InsertDataToMFCDealerSellInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_InsertDataToMFCDealerSellInquiries]
GO

	-- =============================================
-- Author	:	Sachin Bharti(19th Feb 2014)
-- Description	:	Store data regarding the inquiry leads 
--					send to the MFCDealer
-- Modifier		:	Sachin Bharti(29th July 2014)
-- Purpose		:	Update number of leads in DCRM_MFCMappedCities table
--					after lead is sended to the dealer
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_InsertDataToMFCDealerSellInquiries]
	@LeadID		NUMERIC(18,0),
	@Name		VARCHAR(100),
	@Mobile		VARCHAR(50),
	@Phone		VARCHAR(50),
	@Email		VARCHAR(100),
	@City		VARCHAR(100),
	@Make		VARCHAR(50),
	@Model		VARCHAR(50),
	@Variant		VARCHAR(50),
	@ModelYear		VARCHAR(10),
	@RegistrationYear		VARCHAR(10),
	@Color		VARCHAR(50),
	@Kilometer	VARCHAR(50),
	@Owners		VARCHAR(50),
	@RegistrationNumber		VARCHAR(50),
	@RegistrationCity		VARCHAR(50),
	@ReturnResult	NUMERIC(18,0),
	@CityId		INT 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @LeadsSended INT 
    -- Insert statements for procedure here
	INSERT INTO DCRM_MFCDealerSellerInquiries(LeadID,Name,Mobile,Phone,Email,City,Make,Model,Variant,ModelYear,RegistrationYear,Color,Kilometer,Owners
												,RegistrationNumber,RegistrationCity,ReturnResult)
				VALUES(@LeadID,@Name,@Mobile,@Phone,@Email,@City,@Make,@Model,@Variant,@ModelYear,@RegistrationYear,@Color,@Kilometer,@Owners
												,@RegistrationNumber,@RegistrationCity,@ReturnResult)
	IF @@ROWCOUNT <> 0
	BEGIN
		SELECT @LeadsSended = ISNULL(MFC.LeadsSent,0) FROM DCRM_MFCMappedCities MFC(NOLOCK) WHERE CityID = @CityId
		SET @LeadsSended = @LeadsSended + 1
		UPDATE DCRM_MFCMappedCities SET LeadsSent = @LeadsSended  WHERE CityID = @CityId AND IsActive = 1
	END

END

