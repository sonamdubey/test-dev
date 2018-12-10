IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetModelOnBranchId_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetModelOnBranchId_15]
GO
	--	Modifier	:	Sachin Bharti(14th March 2013)
--	Purpose		:	To get all Models on which dealer is working based on its DealerId or BranchId
-- Modified by  :   Raghu on 29-07-2013 to get the order by Price not by text
-- Modified By : Raghu on 2-12-2024 changed query to get models model by text
-- Modified By : Raghu on 2/4/2014 added Models not in for particular dealer
-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
--[TC_GetModelOnBranchId]14132,2
--	============================================================

CREATE PROCEDURE [dbo].[TC_GetModelOnBranchId_15.3.1]

-- CREATED By AMIT KUMAR ON 13- mar 2013
@BranchId	NUMERIC (18,0),
@ApplicationId TINYINT = 1-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify applications.
AS

BEGIN
	SELECT DISTINCT V.ModelId AS Value, V.Model AS Text 
     FROM TC_DealerMakes D WITH(NOLOCK) 
     --INNER JOIN  Dealer_NewCar AS DNC WITH(NOLOCK)  ON DNC.TcDealerId=D.DealerId  -- modified by Sanjay on 25/02/2015
     --INNER JOIN CarModels M WITH(NOLOCK) ON D.MakeId=M.CarMakeId
	 INNER JOIN vwAllMMV V WITH(NOLOCK) ON V.MakeId = D.MakeId -- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application.
     INNER JOIN CarModels CM WITH(NOLOCK) ON CM.ID=V.ModelId-- modeified by vinayak
	 WHERE D.DealerId = @BranchId
	 AND V.New=1
	 AND CM.New=1 and CM.IsDeleted=0--modified by vinayak
     --AND M.Futuristic = 0 
     --AND M.New = 1 
     --AND M.IsDeleted = 0 
	 --AND M.ID NOT IN (Select ModelId from TC_NoDealerModels WITH(NOLOCK) WHERE DealerId = @BranchId ) -- Added by Raghu
	 AND V.ModelId NOT IN (Select ModelId from TC_NoDealerModels WITH(NOLOCK) WHERE DealerId = @BranchId )
	 AND V.ApplicationId = ISNULL(@ApplicationId,1)
     --ORDER BY M.Name
	 ORDER BY V.Model
END

