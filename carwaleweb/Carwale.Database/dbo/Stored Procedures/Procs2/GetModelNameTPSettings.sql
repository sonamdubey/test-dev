IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelNameTPSettings]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelNameTPSettings]
GO

	-- =============================================
-- Author:		Vinayak
-- Create date: 18/05/2016
-- Desc: To rename models as required by third parties api
-- =============================================
CREATE PROCEDURE [dbo].[GetModelNameTPSettings] 
	@ModelId INT
AS
BEGIN
	SELECT RenamedModelName 
	FROM ModelNameTPSettings MTS WITH (NOLOCK)
	where MTS.ModelId=@ModelId
END
