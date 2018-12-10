IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[NCS].[GetDealerIdName]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [NCS].[GetDealerIdName]
GO

	
--Name of SP/Function				: CarWale.dbo.NCS.GetDealerIdName
--Applications using SP				: RM Panel 
--Modules using the SP				: FollowUp.cs
--Technical department				: Database
--Summary							: DropDown Fill
--Author							: Dilip V. 11-Jul-2012
--Modification history				: 1.
CREATE PROCEDURE [NCS].[GetDealerIdName]
@NodeCode	NVARCHAR(4000),
@ExecId		TINYINT
AS 
 BEGIN
	SET NOCOUNT ON
	SELECT DISTINCT ND.Id ,ND.Name
	FROM NCS_DealerOrganization NDO WITH(NOLOCK) 
	INNER JOIN NCS_SubDealerOrganization NSD WITH(NOLOCK) ON NDO.ID = NSD.OId 
	INNER JOIN NCS_RMDealers NRD WITH(NOLOCK) ON NSD.DId = NRD.DealerId 
	INNER JOIN NCS_Dealers ND WITH(NOLOCK) ON NRD.DealerId = ND.ID 
	INNER JOIN NCS_RManagers NRM WITH(NOLOCK) ON NRD.RMId = NRM.Id 
	WHERE NRM.NodeCode LIKE @NodeCode+'%'
	AND NDO.IsCWExecutive = 0 AND NDO.IsActive=1 AND NRM.IsActive = 1 
	AND NRD.IsExecutive = @ExecId
	ORDER BY ND.Name
END
