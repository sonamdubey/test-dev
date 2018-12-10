IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ManageRoleTask]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ManageRoleTask]
GO

	
-- Author:        Binumon George.
-- Create date:   30 Jul 2012
-- Description:   Selecting tasks,roles,task category based on DealerType
-- ====================================================
CREATE PROCEDURE [dbo].[TC_ManageRoleTask]
    -- Add the parameters for the stored procedure here
    @DealerTypeId TINYINT=NULL,
    @RollId INT=NULL
AS
BEGIN
	DECLARE @TaskList VARCHAR(200)
	IF(@DealerTypeId=0)
		BEGIN
			SET @DealerTypeId=1
			SELECT T.Id TaskId, T.TaskName, T.TaskDescription, T.CategoryId 
			FROM TC_Tasks T WHERE T.IsActive = 1 and T.IsVisible=1 AND 
			T.CategoryId <>3 AND (T.TC_DealerTypeId=1 OR TC_DealerTypeId IS NULL) AND IsVisible=1
		END
	 ELSE IF (@DealerTypeId = 3)    -- meaning dealer is both type(UCD and NCD)
            BEGIN
                SELECT Id TaskId, TaskName, TaskDescription, CategoryId, TC_DealerTypeId
                FROM TC_Tasks WHERE IsVisible=1-- retrieving all Tasks
            END
       ELSE IF ( @DealerTypeId<>0)
            BEGIN
                SELECT Id TaskId, TaskName, TaskDescription, CategoryId,TC_DealerTypeId
                FROM TC_Tasks T WHERE (TC_DealerTypeId=@DealerTypeId  OR TC_DealerTypeId IS NULL)
                AND IsVisible=1-- retrieving all Tasks with given dealer type
            END
    
     IF(@RollId IS NOT NULL)--In case of Editing Role    
        BEGIN
            SELECT @TaskList =COALESCE(@TaskList+',' ,'') + convert(VARCHAR,RT.TaskId)
            FROM TC_RoleTasks RT INNER JOIN TC_Roles R ON R.Id=RT.RoleId
            WHERE R.Id=@RollId
           
           SELECT RoleName,@TaskList 'TaskSet',RoleDescription  from TC_Roles where Id= @RollId       
        END  
END


