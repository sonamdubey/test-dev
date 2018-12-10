IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetSalesExecutiveTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetSalesExecutiveTarget]
GO

	/*
Created By:Vishal Srivastava-AE1830
Date:September 10,2013
Purpose: To get the tables for target type and names, ids of sales executive AND CARMODELS
*/


CREATE PROCEDURE [dbo].[TC_GetSalesExecutiveTarget]
@BranchId INT,
@MakeId INT,
@Month TINYINT,
@Year SMALLINT,
@TargetType SMALLINT,
@CurrentUserId INT


AS
BEGIN
/*
   get target type for dropdown in the page ddlTargetType
*/
SELECT TC_TargetTypeId, TargetType FROM TC_TargetType WITH (NOLOCK) WHERE TC_PanelTypeId=1 AND IsActive=1;

/*
	get the user's id and name who are
	allowed comes under certain branch user who all are permitted
*/
SELECT DISTINCT U.Id, U.UserName
  FROM TC_Users AS U WITH (NOLOCK)
     INNER JOIN TC_UserModelsPermission  AS TCUM WITH (NOLOCK)
                                        ON TCUM.TC_UsersId=U.Id
     WHERE  U.IsActive=1
       AND  U.BranchId=@BranchId                                  
 ORDER BY U.Id;

 /*
	get the model's id and name who are
	allowed comes under certain car company
	and is active model
 */
SELECT ID,Name FROM CarModels WITH (NOLOCK) WHERE IsDeleted = 0 
			AND Futuristic = 0 
			AND  CarMakeId =@MakeId
			AND New = 1 

/*
	get the permission of the model that a user can sell
*/
SELECT TC_UsersId, ModelId 
FROM TC_UserModelsPermission AS TCUM WITH (NOLOCK) 
 JOIN TC_Users AS U  WITH (NOLOCK) ON U.Id=TCUM.TC_UsersId
  WHERE U.IsActive=1
   AND U.BranchId=@BranchId
ORDER BY TCUM.TC_UsersId;

/*
	gets the target value for a user against a car model
	for certain year month and userid
*/
SELECT TC_UserModelsTargetId,TC_UsersId,CarModelId,[Month],[Year],[Target],CreatedBy,TC_TargetTypeId
FROM TC_UserModelsTarget WITH (NOLOCK)
WHERE [Month]=@Month 
AND [Year]=@Year 
AND CreatedBy=@CurrentUserId
AND IsDeleted=0
AND TC_TargetTypeId=@TargetType;

/*
Sum of all targets from the database
*/
SELECT SUM([Target]) AS Total,CarModelId 
from TC_UserModelsTarget AS TUM WITH (NOLOCK) 
JOIN TC_Users AS U WITH (NOLOCK) ON U.Id=TUM.TC_UsersId
where [Month]=@Month 
and [Year]=@Year 
and TC_TargetTypeId=@TargetType 
and U.BranchId=@BranchId
and IsDeleted=0
group by CarModelId

/*
Grand Sum
*/
select SUM([Target]) from TC_UserModelsTarget  AS TUM WITH (NOLOCK) 
JOIN TC_Users AS U WITH (NOLOCK) ON U.Id=TUM.TC_UsersId
where [Month]=@Month 
and [Year]=@Year 
and TC_TargetTypeId=@TargetType
and U.BranchId=@BranchId
and IsDeleted=0

END
