IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerDetails_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerDetails_V16]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 17th Dec 2015
-- Description : To get dealer details 
-- Modified By : Ashwini Dhamankar on Oct 14,2016 (Fetched ApplicationType and DealerType)
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealerDetails_V16.10.1]
	@DealerId INT
AS
BEGIN
	SELECT D.Organization,D.MobileNo,EmailId,D.Address1 + ',' + D.Address2 AS Address
	,A.ApplicationName AS ApplicationType,DT.DealerType  -- Modified By : Ashwini Dhamankar on Oct 14,2016  
	FROM Dealers D WITH(NOLOCK)
	INNER JOIN TC_DealerType DT WITH(NOLOCK) ON D.TC_DealerTypeId = DT.TC_DealerTypeId 
	INNER JOIN TC_Applications A WITH(NOLOCK) ON D.ApplicationId = A.ApplicationId
	WHERE ID = @DealerId
END
