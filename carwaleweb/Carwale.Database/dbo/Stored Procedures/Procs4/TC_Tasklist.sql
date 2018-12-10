IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Tasklist]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Tasklist]
GO

	-- ====================================================
-- Modified By:    Tejashree Patil.
-- Modified Date:  12/4/2012
-- Description:    Changed For Normal UCD dealer of type null.
-- ====================================================
-- Author:        Tejashree Patil.
-- Create date:   2/4/2012
-- Description:   Selecting tasks,roles,task category based on DealerType
-- ====================================================
CREATE PROCEDURE [dbo].[TC_Tasklist]
    -- Add the parameters for the stored procedure here
    @DealerTypeId TINYINT=NULL,
    @RollId INT=NULL
AS
BEGIN
    IF(@DealerTypeId=0)
        BEGIN
            SET @DealerTypeId=1
            SELECT TC.Id 'CategoryId', TC.CategoryName + ' '+ ISNULL(CategoryDescription,'')  AS CategoryName FROM TC_TaskCategory TC
            WHERE TC.IsActive=1 AND TC.Id <>3 AND(TC_DealerTypeId=1 OR TC_DealerTypeId IS NULL)

            SELECT Id TaskId, TaskName, TaskDescription, CategoryId, TC_DealerTypeId FROM TC_Tasks T
            WHERE CategoryId <>3 AND (TC_DealerTypeId=1 OR TC_DealerTypeId IS NULL) AND IsVisible=1 AND CategoryId !=4
        END
    ELSE
        IF (@DealerTypeId = 3)    -- meaning dealer is both type(UCD and NCD)
            BEGIN
                SELECT TC.Id 'CategoryId', TC.CategoryName  + ' ' + ISNULL(CategoryDescription,'')  AS CategoryName FROM TC_TaskCategory TC WHERE TC.IsActive=1 -- retrieving all category
                
                SELECT Id TaskId, TaskName, TaskDescription, CategoryId, TC_DealerTypeId FROM TC_Tasks WHERE IsVisible=1 AND SubCategoryId IS NULL AND CategoryId !=4-- retrieving all Tasks
                ---SELECT Id TaskId, TaskName, TaskDescription, CategoryId, TC_DealerTypeId FROM TC_Tasks group by CategoryId, TC_DealerTypeId,TaskName,Id,TaskDescription , IsVisible  -- retrieving all Tasks
            END
        ELSE
            IF ( @DealerTypeId<>0)
                BEGIN
                    SELECT TC.Id 'CategoryId', TC.CategoryName  + ' ' + ISNULL(CategoryDescription,'')  AS CategoryName FROM TC_TaskCategory TC WHERE TC.IsActive=1 AND (TC_DealerTypeId=@DealerTypeId OR TC_DealerTypeId IS NULL) -- retrieving all category with type and null
                    SELECT Id TaskId, TaskName, TaskDescription, CategoryId,TC_DealerTypeId FROM TC_Tasks T WHERE (TC_DealerTypeId=@DealerTypeId -- retrieving all Tasks with given dealer type
                    OR TC_DealerTypeId IS NULL) AND IsVisible=1 AND CategoryId !=4
                END

    IF(@RollId IS NOT NULL)--In case of Editing Role    
        BEGIN
            DECLARE @TaskList VARCHAR(200)
            SELECT @TaskList =COALESCE(@TaskList+',' ,'') + convert(VARCHAR,RT.TaskId)
            FROM TC_RoleTasks RT INNER JOIN TC_Roles R ON R.Id=RT.RoleId
            WHERE R.Id=@RollId
           
            SELECT RoleName,@TaskList 'TaskSet',RoleDescription FROM TC_Roles WHERE Id= @RollId           
        END
END