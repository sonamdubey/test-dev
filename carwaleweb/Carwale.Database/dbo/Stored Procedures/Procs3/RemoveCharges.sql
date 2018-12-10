IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RemoveCharges]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RemoveCharges]
GO

	
-- =============================================
-- Author:		Vicky Lund
-- Create date: 22/06/2016
-- EXEC [RemoveCharges] '33'
-- =============================================
CREATE PROCEDURE [dbo].[RemoveCharges] @CategoryIds VARCHAR(5000)
AS
BEGIN
	UPDATE PQ_CategoryItems
	SET IsActive = 0
	WHERE Id IN (
			SELECT ListMember
			FROM dbo.fnSplitCSVMAx(@CategoryIds)
			)
END

