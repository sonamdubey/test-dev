IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchBMWData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchBMWData]
GO

	-- =============================================
-- Author      : Chetan Navin		
-- Create date : 11 Sep 2013
-- Description : To Fetch BMW Data to push
-- Modified by : Chetan Dev/ Manish on 22-10-2013 for checking the cbd id input is coming twice or not if twice then return @DealerNameValue null.
-- =============================================
CREATE PROCEDURE [dbo].[CRM_FetchBMWData]
	--Parameters
	@CBDId				BIGINT,
	@Salutation			VARCHAR(5) OUTPUT,
	@SalutationValue	VARCHAR(100) OUTPUT,
	@FirstName			VARCHAR(25) OUTPUT,
	@LastName			VARCHAR(25) OUTPUT,
	@Mobile				VARCHAR(15) OUTPUT,
	@AlternateContact	VARCHAR(15) OUTPUT,
	@Email				VARCHAR(25) OUTPUT,
	@DealerNameValue	VARCHAR(100) OUTPUT,
	@DealerId			NUMERIC OUTPUT,
	@City				VARCHAR(50) OUTPUT,
	@MakeNameValue		VARCHAR(100) OUTPUT,
	@ModelNameValue		VARCHAR(100) OUTPUT,
	@VersionNameValue	VARCHAR(100) OUTPUT,
	@MakeId				Numeric OUTPUT,
	@ModelId			Numeric OUTPUT,
	@VersionId			Numeric OUTPUT,
	@PurchaseIntention	VARCHAR(100) OUTPUT,
	@PurchaseIntentionValue VARCHAR(100) OUTPUT,
	@OtherCars          VARCHAR(100) OUTPUT,
	@OtherBMWCars		VARCHAR(100) OUTPUT,
	@GeneralRemarks		VARCHAR(5000) OUTPUT,
	@IsFinaceRequired	VARCHAR(5)	OUTPUT,
	@LookingFor			VARCHAR(20) OUTPUT,
	@TradeCarMake		VARCHAR(50) OUTPUT,
	@TradeCarModel		VARCHAR(50) OUTPUT,
	@TradeCarVersion	VARCHAR(50) OUTPUT,
	@TradeCarYear		VARCHAR(20) OUTPUT,
	@TradeCarKms		VARCHAR(20) OUTPUT,
	@CurrentCarOwned	VARCHAR(250) OUTPUT
AS

BEGIN


     IF  (NOT EXISTS (SELECT CBDID FROM  CRM_BMWCheckCBDId WHERE CBDId=@CBDId) AND @CBDId IS NOT NULL)
   BEGIN 

		   INSERT INTO  CRM_BMWCheckCBDId (CBDId) VALUES (@CBDId);

		SELECT @SalutationValue = CASE CU.Salutation 
								WHEN 'Mrs.' THEN '30E544DC-B748-E111-B8E1-005056820025'
								WHEN 'Mr.' THEN '2EE544DC-B748-E111-B8E1-005056820025'
								WHEN 'Dr.' THEN '2CE544DC-B748-E111-B8E1-005056820025'
								WHEN 'Ms.' THEN '32E544DC-B748-E111-B8E1-005056820025'
							END,
		@Salutation = CU.Salutation,
		@FirstName = CU.FirstName,@LastName = CU.LastName,@Mobile = CU.Mobile,
		@AlternateContact = CASE LEN(CU.AlternateContactNo) WHEN 10 THEN CU.AlternateContactNo ELSE CU.Mobile END,@Email = CU.Email,@City = C.Name,
		@DealerNameValue = D.DealerCode, @DealerId = D.Id,
		@MakeNameValue = CASE VW.MakeId WHEN 1 THEN '6D714D61-9543-E111-960E-005056820025' WHEN 51 THEN '0DB0471A-4847-E111-8207-005056820025' END,
		@MakeId = VW.MakeId,
		@ModelNameValue = CASE	WHEN VW.ModelId = 126 THEN '2A5DB9FE-F4DF-E211-8672-005056820025' --1 Series
								WHEN VW.ModelId = 489 THEN '35027CC5-564D-E111-957C-005056820025' -- X1
								WHEN VW.ModelId = 439 AND VW.VersionId = 2960 THEN 'A7BE48B2-6685-E111-9129-005056820025' -- CountryMen
								WHEN VW.ModelId = 439 AND VW.VersionId = 3102 THEN 'A7BE48B2-6685-E111-9129-005056820025'  -- CountryMen
								WHEN VW.ModelId = 439 AND VW.VersionId = 2961 THEN '3284A63B-C1BB-E211-8672-005056820025' END, 		-- CountryMenHigh				
		@ModelId = VW.ModelId, 
		@VersionNameValue = CASE VW.VersionId	WHEN 739 THEN '2A5DB9FE-F4DF-E211-8672-005056820025' --116i 1-Series
												WHEN 741 THEN '2A5DB9FE-F4DF-E211-8672-005056820025' -- 118d
												WHEN 3080 THEN '2A5DB9FE-F4DF-E211-8672-005056820025' --118d Sport Line
												WHEN 3081 THEN '2A5DB9FE-F4DF-E211-8672-005056820025' --118d Sport Plus
											
												WHEN 2825 THEN '35027CC5-564D-E111-957C-005056820025' --sDrive20d Sport Line X1
												WHEN 2824 THEN '35027CC5-564D-E111-957C-005056820025' --sDrive20d
												WHEN 2826 THEN '35027CC5-564D-E111-957C-005056820025' --sDrive20d xLine
											
												WHEN 2960 THEN 'A7BE48B2-6685-E111-9129-005056820025' --CountryMen Cooper D
												WHEN 3102 THEN 'A7BE48B2-6685-E111-9129-005056820025' --CountryMen Cooper S
												WHEN 2961 THEN '3284A63B-C1BB-E211-8672-005056820025' -- Countrymen High Cooper D
						END,
		@VersionId = VW.VersionId,
		@PurchaseIntention = CVO.PurchaseTime,
		@PurchaseIntentionValue = CASE CVO.PurchaseTime WHEN 7 THEN '174640004' WHEN 14 THEN '174640004' WHEN 30 THEN '174640004' WHEN 60 THEN '174640000' WHEN 70 THEN '174640000' ELSE '174640001' END, 
		@GeneralRemarks = CVL.Comments,
		@IsFinaceRequired = CASE CVO.PurchaseMode WHEN 1 THEN 'true' ELSE 'false' END,
		@TradeCarMake = VWM.Make,@TradeCarModel = VWM.Model, @TradeCarVersion = VWM.Version,@TradeCarYear = CS.MakeYear,@TradeCarKms = CS.Kilometers,
		@LookingFor = CASE ISNULL(VWM.MakeId,0) WHEN 0 THEN '' ELSE 'Exchange' END,
		@CurrentCarOwned = CVO.CurrentCarOwned
		FROM CRM_VerificationLog CVL
		INNER JOIN CRM_Leads CL ON CL.ID = CVL.LeadId
		INNER JOIN CRM_Customers CU ON CU.ID = CL.CNS_CustId
		INNER JOIN CRM_VerificationOthersLog CVO ON CVO.LeadId = CL.ID
		INNER JOIN vwMMV VW ON  VW.VersionId = CVL.VersionId
		INNER JOIN Cities C ON CU.CityId = C.ID 
		INNER JOIN NCS_Dealers D ON CVL.DealerId = D.ID
		LEFT JOIN CRM_CrossSellInquiries CSI ON CSI.LeadId = CL.ID
		LEFT JOIN CustomerSellInquiries CS ON CS.ID = CSI.SelInquiryId
		LEFT JOIN vwMMV VWM ON VWM.VersionId = CS.CarVersionId 
		WHERE CVL.CBDId = @CBDId 

	END 
  ELSE
    BEGIN 
	  SET @DealerNameValue=NULL
	--SELECT @DealerNameValue

	END 



END