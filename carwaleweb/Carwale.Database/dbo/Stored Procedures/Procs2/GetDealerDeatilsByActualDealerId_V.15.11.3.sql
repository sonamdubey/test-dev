IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerDeatilsByActualDealerId_V]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerDeatilsByActualDealerId_V]
GO

	CREATE PROCEDURE [dbo].[GetDealerDeatilsByActualDealerId_V.15.11.3] @ActualDealerId INT
	,@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(250) OUTPUT
	,@DealerActualMobile VARCHAR(100) OUTPUT
	,@DealerLeadBusinessType INT OUTPUT
	,@DealerAddress VARCHAR(MAX) OUTPUT
	,@LeadPanelDealerId INT OUTPUT
	,@LeadPanelDealerMobile VARCHAR(50) OUTPUT ----Parameter added by Sourav on 29-05-2015
	,@LeadPanelDealerEmail VARCHAR(250) OUTPUT ----Parameter added by Sourav on 29-05-2015
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @LeadPanelDealerId = dl.ID
		,@DealerName = dl.Organization
		,@DealerMobile = dl.ActiveMaskingNumber
		,@DealerEmail = dl.EmailId
		,@DealerActualMobile = dl.MobileNo
		,@LeadPanelDealerMobile = dl.MobileNo
		,@LeadPanelDealerEmail = dl.EmailId
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType --modified by ashish verma
		,@DealerAddress = isnull(dl.Address1, ' ') + isnull(dl.Address2, ' ') + isnull(' ' + C.NAME, ' ')
	FROM Dealers dl WITH (NOLOCK)
	LEFT JOIN Cities C WITH (NOLOCK) ON dl.CityId = C.ID
	WHERE dl.Id = @ActualDealerId
END
 

