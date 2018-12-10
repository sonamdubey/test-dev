IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetTaskCategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetTaskCategories]
GO

	-- Author:        Binu.
-- Create date:   30 JUL 2012
-- Description:   Geting all active task categories
-- ====================================================
CREATE PROCEDURE [dbo].[TC_GetTaskCategories]
    @DealerTypeId TINYINT=NULL
AS
BEGIN
	  IF(@DealerTypeId=0)
        BEGIN
            SET @DealerTypeId=1
			SELECT TC.Id 'CategoryId', TC.CategoryName,TC.CategoryDescription FROM TC_TaskCategory TC
			WHERE TC.IsActive=1 AND TC.Id <>3 AND(TC_DealerTypeId=1 OR TC_DealerTypeId IS NULL)
		END
		ELSE IF (@DealerTypeId = 3)    -- meaning dealer is both type(UCD and NCD)
            BEGIN
                SELECT TC.Id 'CategoryId', TC.CategoryName,TC.CategoryDescription FROM TC_TaskCategory TC WHERE TC.IsActive=1 -- retrieving all category
            END
        ELSE IF (@DealerTypeId<>0)
            BEGIN
                SELECT TC.Id 'CategoryId', TC.CategoryName,TC.CategoryDescription FROM TC_TaskCategory TC WHERE TC.IsActive=1 AND (TC_DealerTypeId=@DealerTypeId OR TC_DealerTypeId IS NULL) -- retrieving all category with type and null
            END
END


