IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetModelOnBranchId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetModelOnBranchId]
GO

	
--	Modifier	:	Sachin Bharti(14th March 2013)
--	Purpose		:	To get all Models on which dealer is working based on its DealerId or BranchId
-- Modified by  :   Raghu on 29-07-2013 to get the order by Price not by text
-- Modified By : Raghu on 2-12-2024 changed query to get models model by text
-- Modified By : Raghu on 2/4/2014 added Models not in for particular dealer
-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application and joined with vwAllMMV view.
-- Modified By vivek gupta on 09-01-2016, commented Dealer_newCar join coz it doesen exists now
--[TC_GetModelOnBranchId]14132,2
--	============================================================

CREATE PROCEDURE [dbo].[TC_GetModelOnBranchId]

-- CREATED By AMIT KUMAR ON 13- mar 2013
@BranchId	INT,
@ApplicationId TINYINT = 1-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify applications.
AS

BEGIN
	SELECT DISTINCT V.ModelId AS Value, V.Model AS Text 
     FROM TC_DealerMakes D WITH(NOLOCK) 
     --INNER JOIN  Dealer_NewCar AS DNC WITH(NOLOCK)  ON DNC.TcDealerId=D.DealerId  -- modified by Sanjay on 25/02/2015
     --INNER JOIN CarModels M WITH(NOLOCK) ON D.MakeId=M.CarMakeId
	 INNER JOIN vwAllMMV V WITH(NOLOCK) ON V.MakeId = D.MakeId -- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application.
     WHERE D.DealerId = @BranchId
	 AND V.New=1
     --AND M.Futuristic = 0 
     --AND M.New = 1 
     --AND M.IsDeleted = 0 
	 --AND M.ID NOT IN (Select ModelId from TC_NoDealerModels WITH(NOLOCK) WHERE DealerId = @BranchId ) -- Added by Raghu
	 AND V.ModelId NOT IN (Select ModelId from TC_NoDealerModels WITH(NOLOCK) WHERE DealerId = @BranchId )
	 AND V.ApplicationId = ISNULL(@ApplicationId,1)
     --ORDER BY M.Name
	 ORDER BY V.Model
END

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------














/****** Object:  StoredProcedure [dbo].[TC_FetchLostLeadDetails]    Script Date: 1/13/2016 12:01:42 PM ******/
SET ANSI_NULLS ON
