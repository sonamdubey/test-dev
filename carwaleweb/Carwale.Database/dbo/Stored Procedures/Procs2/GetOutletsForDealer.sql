IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOutletsForDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOutletsForDealer]
GO

	
--===========================================
--Created By: Kartik Rathod on 1 Dec 2015,To get Outlets List For Multi-Admin dealers
--Modified by : Amit Yadav (2 Feb 2016) 
--Purpose : Chanege whole SP to get the multioutlets mapped to a group and outlets mapped to the group's multioutlets.
-- EXEC GetOutletsForDealer 12055
--===========================================


CREATE PROC [dbo].[GetOutletsForDealer]
@DealerId INT
AS 
BEGIN
	SET NOCOUNT ON;
	--To get the multioutlets and their Id
	/*SELECT D.Organization AS MultioutletName,DAM.DealerId AS MultioutletId 
	FROM TC_DealerAdmin AS DA WITH(NOLOCK)
	LEFT JOIN TC_DealerAdminMapping AS DAM WITH(NOLOCK) ON  DA.Id=DAM.DealerAdminId
	LEFT JOIN Dealers AS D WITH(NOLOCK) ON D.Id=DAM.DealerId
	WHERE DA.DealerId = @DealerId AND DAM.IsGroup=1 AND DA.IsActive=1*/

	--To get the multioutletIds of the group and use it to get the outlets
	DECLARE @TempMultioutletsId TABLE(RowId INT IDENTITY(1,1), MultioutletName VARCHAR(100),MultioutletId INT) 
	
	INSERT INTO @TempMultioutletsId
	SELECT D.Organization AS MultioutletName,DAM.DealerId AS MultioutletId 
	FROM TC_DealerAdmin AS DA WITH(NOLOCK)
	LEFT JOIN TC_DealerAdminMapping AS DAM WITH(NOLOCK) ON  DA.Id=DAM.DealerAdminId
	LEFT JOIN Dealers AS D WITH(NOLOCK) ON D.Id=DAM.DealerId
	WHERE DA.DealerId = @DealerId AND ISNULL(DAM.IsGroup,0) = 0 AND DA.IsActive = 1  AND (D.IsMultiOutlet <> 0 OR D.IsGroup <> 0)

	SELECT RowId,MultioutletName,MultioutletId FROM @TempMultioutletsId


	--To get the outlets,outletsId and their respective multioutletsId
	SELECT  D.Organization AS Text, DAM.DealerId AS Value, DA.DealerId AS MulDealerId
	FROM	TC_DealerAdmin AS DA WITH(NOLOCK)
			INNER JOIN @TempMultioutletsId TT ON TT.MultioutletId =DA.DealerId
			LEFT JOIN TC_DealerAdminMapping AS DAM WITH(NOLOCK) ON  DA.Id=DAM.DealerAdminId
			LEFT JOIN Dealers AS D WITH(NOLOCK) ON D.Id=DAM.DealerId
	WHERE ISNULL(DAM.IsGroup, 0)=0 AND DA.IsActive=1 AND DAM.DealerId <> TT.MultioutletId

END
