IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckDealerBelongtoMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckDealerBelongtoMake]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 12 Sept,2013
-- Description:	Get dealer details on makeId to identify dealer mapped with make.
-- [TC_CheckDealerBelongtoMake] null ,null
-- =============================================
CREATE PROCEDURE [dbo].[TC_CheckDealerBelongtoMake]
@BranchId BIGINT,
@MakeId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--Get DealerCode
	SELECT	D.Organization, D.ID,D.TC_BrandZoneId,D.DealerCode
	FROM	Dealers D  WITH(NOLOCK)
			INNER JOIN	TC_BrandZone BZ WITH(NOLOCK) ON 
						BZ.TC_BrandZoneId=D.TC_BrandZoneId
	WHERE	IsTCDealer=1
			AND BZ.IsActive=1
			AND D.IsDealerActive=1
			AND (@BranchId IS NULL OR D.Id=@BranchId)
			AND (@MakeId IS NULL OR BZ.MakeId=@MakeId )
END
