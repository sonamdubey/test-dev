IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerDeatilsByActualDealerId_V]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerDeatilsByActualDealerId_V]
GO

	-- =============================================
-- Author: Satish Sharma
-- Create date: 07/08/2014
-- Description:	Get dealer Details based on dealerId
-- exec [dbo].[GetDealerDeatilsByActualDealerId]
--modified by rohan sapkal , fetch address with city from Dealer_NewCar instead of Dealers (if available) 11-05-2015
--modified by Sourav Roy , fetch LeadPanelDealerId from Dealer_NewCar 28-05-2015
--modified by  vinayak on 15-07-2015, LeadPanelDealerId to discard the zero valued records
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerDeatilsByActualDealerId_V.15.5.2] 
	@ActualDealerId INT
	,@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(250) OUTPUT
	,@DealerActualMobile VARCHAR(100) OUTPUT
	,@DealerLeadBusinessType INT OUTPUT
	,@DealerAddress VARCHAR(MAX) OUTPUT
	,@LeadPanelDealerId INT OUTPUT
	,@LeadPanelDealerMobile  VARCHAR(50) OUTPUT ----Parameter added by Sourav on 29-05-2015
	,@LeadPanelDealerEmail VARCHAR(250) OUTPUT  ----Parameter added by Sourav on 29-05-2015
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT @DealerName = dl.Organization
		,@DealerMobile = dl.ActiveMaskingNumber
		,@DealerEmail = dl.EmailId
		,@DealerActualMobile = dl.MobileNo
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType --modified by ashish verma
		,@DealerAddress = isnull(dl.Address1, ' ') + isnull(dl.Address2, ' ')+ isnull(' '+C.Name, ' ')
	FROM Dealers dl WITH (NOLOCK)
	LEFT JOIN Cities C WITH(NOLOCK) on dl.CityId =C.ID
	WHERE dl.Id = @ActualDealerId

	DECLARE @TempAddress VARCHAR(250)=NULL
	
	SELECT TOP 1 @TempAddress = DN.Address + ' ' + C.Name
	       ,@LeadPanelDealerId=ISNULL(DN.LeadPanelDealerId,0)
	FROM Dealer_NewCar DN WITH(NOLOCK) 
	LEFT JOIN Cities C WITH(NOLOCK) ON DN.CityId=C.ID
	WHERE DN.TcDealerId=@ActualDealerId order by LeadPanelDealerId desc

	IF(@TempAddress<>NULL) 
	BEGIN
	   SET @DealerAddress=@TempAddress
	END 
	
	IF(@LeadPanelDealerId > 0)
	BEGIN
		SELECT @LeadPanelDealerMobile = d2.MobileNo
			  ,@LeadPanelDealerEmail = d2.EmailId
		FROM Dealers d2 WITH (NOLOCK)
		WHERE d2.Id = @LeadPanelDealerId
	END 
	
END
