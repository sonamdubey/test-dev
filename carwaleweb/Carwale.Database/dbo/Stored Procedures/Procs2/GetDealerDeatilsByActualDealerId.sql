IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerDeatilsByActualDealerId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerDeatilsByActualDealerId]
GO

	
-- =============================================
-- Author: Satish Sharma
-- Create date: 07/08/2014
-- Description:	Get dealer Details based on dealerId
-- exec [dbo].[GetDealerDeatilsByActualDealerId]
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerDeatilsByActualDealerId] @ActualDealerId INT
	,@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(250) OUTPUT
	,@DealerActualMobile VARCHAR(100) OUTPUT
	,@DealerLeadBusinessType INT OUTPUT
	,@DealerAddress VARCHAR(MAX) OUTPUT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	--SELECT DealerId,DealerName,Phone AS PhoneNo,DealerEmailId AS DealerEmail
	--FROM PQ_DealerSponsored WITH(NOLOCK)
	--WHERE CityId= @CityId and ModelId=@ModelId and IsActive = 1
	SELECT @DealerName = dl.Organization
		,@DealerMobile = dl.ActiveMaskingNumber
		,@DealerEmail = dl.EmailId
		,@DealerActualMobile = dl.MobileNo
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType --modified by ashish verma
		,@DealerAddress = isnull(dl.Address1, ' ') + isnull(dl.Address2, ' ')
	FROM Dealers dl WITH (NOLOCK)
	WHERE dl.Id = @ActualDealerId
END
