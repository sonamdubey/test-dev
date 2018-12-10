IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetMasterDealerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetMasterDealerData]
GO
	 
-- =============================================
-- Author		: Nilima More,	
-- Create date  : 23 Feb 2016
-- Description  : To Master dealer data for group users.
-- EXEC TC_GetMasterDealerData 299 --GROUPDealerid
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetMasterDealerData] 
	@GroupDealerId INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
				SELECT  D.Organization,D.ID
				FROM TC_DealerAdmin DA WITH(NOLOCK)
				INNER JOIN TC_DealerAdminMapping DAM  WITH(NOLOCK) ON DA.Id = DAM.DealerAdminId
				INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = DAM.DealerId
				WHERE DA.DealerId = @GroupDealerId  AND D.IsMultiOutlet = 1 AND DA.IsActive = 1 AND IsDealerActive = 1 AND IsDealerDeleted = 0 
				ORDER BY Organization
END
-----------------------------------------------------------------------------------------------------------------------------------------------------
