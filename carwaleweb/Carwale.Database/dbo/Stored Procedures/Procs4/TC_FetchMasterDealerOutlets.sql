IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchMasterDealerOutlets]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchMasterDealerOutlets]
GO

	--=======================================================================
--Author : Tejashree Patil on 26 Oct 2016, To get master's all outlets
--=======================================================================
CREATE PROCEDURE [dbo].[TC_FetchMasterDealerOutlets]
	@DealerId INT
AS
BEGIN
	SELECT  D.Organization AS Value, D.Id Id ,  D.TC_DealerTypeId DealerTypeId
	FROM TC_DealerAdmin DA WITH(NOLOCK)
	INNER JOIN TC_DealerAdminMapping DAM  WITH(NOLOCK) ON DA.Id = DAM.DealerAdminId
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = DAM.DealerId
	WHERE Da.id in (select Dealeradminid from TC_DealerAdminMapping D WHERE D.dealerid = @DealerId)   
	--AND D.IsMultiOutlet = 1 AND DA.IsActive = 1 AND IsDealerActive = 1 AND IsDealerDeleted = 0
	AND D.TC_DealerTypeId IN (5,8)
END