IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertSalesExecutiveTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertSalesExecutiveTarget]
GO

	/*
Created By:Vishal Srivastava-AE1830
Date:September 23,2013
Purpose: To insert or update the target of Users
*/

CREATE PROCEDURE [dbo].[TC_InsertSalesExecutiveTarget]
@myTableType TC_UserModelsTargetType READONLY
AS
BEGIN
		
		UPDATE U
		SET U.[Target]=M.Target,
		U.CreatedBy=M.CreatedBy
		FROM TC_UserModelsTarget AS U
		  INNER JOIN @myTableType AS M
		ON  U.TC_UserModelsTargetId=M.TC_UserModelsTargetId
		AND U.IsDeleted=0


INSERT INTO TC_UserModelsTarget (TC_UsersId,CarModelId,[Month],[Year],[Target],CreatedBy,TC_TargetTypeId)
        SELECT     M.TC_UsersId,M.CarModelId,M.[Month],M.[Year],M.[Target],M.CreatedBy,M.TC_TargetTypeId 
        FROM  @myTableType AS M
         WHERE M.TC_UserModelsTargetId=-1 
		 

END
