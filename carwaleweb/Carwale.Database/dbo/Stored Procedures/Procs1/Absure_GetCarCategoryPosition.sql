IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetCarCategoryPosition]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetCarCategoryPosition]
GO

	-- =============================================
-- Author:        Ruchira Patil
-- Create date: 11th Mar 2015
-- Description:    To fetch car caregory and position
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetCarCategoryPosition]
AS
BEGIN
    SELECT AQP.AbSure_QPositionId AS PositionId,AQP.Position
    FROM AbSure_QPosition AQP WITH(NOLOCK)
    WHERE AQP.IsActive=1


    SELECT AQC.AbSure_QCategoryId AS CategoryId,AQC.Category,AQC.Sequence
    FROM AbSure_QCategory AQC WITH(NOLOCK)
    WHERE AQC.IsActive=1
END
------------------------------------------------------------------------------------------------------------------------------------------------------------
