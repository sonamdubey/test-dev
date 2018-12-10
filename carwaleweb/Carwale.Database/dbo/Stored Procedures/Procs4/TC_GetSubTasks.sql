IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetSubTasks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetSubTasks]
GO

	-- Author:        Binumon George.
-- Create date:   03/5/2012
-- Description:   Geting sub task 
-- ====================================================
CREATE PROCEDURE [dbo].[TC_GetSubTasks]
    -- Add the parameters for the stored procedure here
    @DealerTypeId TINYINT=NULL
AS
BEGIN
	IF(@DealerTypeId IS NULL OR @DealerTypeId=0)-- Normal dealers
		BEGIN
			SELECT Id TaskId, TaskName, TaskDescription, CategoryId, TC_DealerTypeId, SubCategoryId, 
			CASE SubCategoryId 
				WHEN 1 THEN 'Used Car Inquiries'
				WHEN 2 THEN 'New Car Inquiries'
				ELSE 'Service/Maintenance Request'
			END AS SubCategoryName
				FROM TC_Tasks 
				WHERE IsVisible=1 AND SubCategoryId IN(1)
		END
	ELSE IF(@DealerTypeId IS NOT NULL AND @DealerTypeId=1)-- ucd dealers
		BEGIN
			SELECT Id TaskId, TaskName, TaskDescription, CategoryId, TC_DealerTypeId, SubCategoryId, 
			CASE SubCategoryId 
				WHEN 1 THEN 'Used Car Inquiries'
				--WHEN 2 THEN 'New Car Inquiries'
				--ELSE 'Service/Maintenance Request'
			END AS SubCategoryName
				FROM TC_Tasks 
				WHERE IsVisible=1 AND SubCategoryId IN(1)
		END
	ELSE IF(@DealerTypeId IS NOT NULL AND @DealerTypeId=2)-- ncd dealers
		BEGIN
			SELECT Id TaskId, TaskName, TaskDescription, CategoryId, TC_DealerTypeId, SubCategoryId, 
			CASE SubCategoryId 
				--WHEN 1 THEN 'Used Car Inquiries'
				WHEN 2 THEN 'New Car Inquiries'
				ELSE 'Service/Maintenance Request'
			END AS SubCategoryName
				FROM TC_Tasks 
				WHERE IsVisible=1 AND SubCategoryId IN(2,3)
		END
		ELSE IF(@DealerTypeId IS NOT NULL AND @DealerTypeId=3)--both ucd and ncd dealers
		BEGIN
			SELECT Id TaskId, TaskName, TaskDescription, CategoryId, TC_DealerTypeId, SubCategoryId, 
			CASE SubCategoryId 
				WHEN 1 THEN 'Used Car Inquiries'
				WHEN 2 THEN 'New Car Inquiries'
				ELSE 'Service/Maintenance Request'
			END AS SubCategoryName
				FROM TC_Tasks 
				WHERE IsVisible=1 AND SubCategoryId IN(1,2,3)
		END
END
