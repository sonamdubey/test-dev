IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelsOnMakeIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelsOnMakeIds]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 23/06/2016
-- Description:	Returns models for multiple makeIds(comma-separated makeIds)
-- exec GetModelsOnMakeIds '18,1'
-- Modified : Vicky Lund, 25/07/2016, Get All models case added
-- =============================================
CREATE PROCEDURE [dbo].[GetModelsOnMakeIds]
	-- Add the parameters for the stored procedure here
	@MakeIds VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT ID AS Value
		,NAME AS [Text]
	FROM carmodels WITH (NOLOCK)
	WHERE (
			CarMakeId IN (
				SELECT listmember
				FROM fnSplitCSV(@MakeIds)
				)
			OR @MakeIds = '-1'
			)
		AND New = 1
		AND IsDeleted = 0
	ORDER BY NAME
END

