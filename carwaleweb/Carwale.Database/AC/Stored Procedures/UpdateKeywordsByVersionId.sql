IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[AC].[UpdateKeywordsByVersionId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [AC].[UpdateKeywordsByVersionId]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [ac].[UpdateKeywordsByVersionId]
	-- Add the parameters for the stored procedure here
	@VersionId INT,
	@Action TINYINT --	1 = INSERT, 2 = UPDATE, 3 = DELETE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF(@Action = 1)
	BEGIN
		INSERT INTO [ac].[SRC_Keywords] ([Name],[KeywordTypeId],[ReferenceId],[DisplayName],[IsNew],[IsUsed],[Value],[IsAutomated])
		SELECT [ac].[RSC_ExceptSpaces](CMA.Name + ' ' + CMO.Name + ' ' + CV.Name),
			5,
			CV.ID,
			CMA.Name + ' ' + CMO.Name + ' ' + CV.Name DN,
			CV.New,
			CV.Used,
			ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100))+'|'+CMO.MaskingName+':'+cast(CMO.Id as varchar(100))
			+'|'+ac.RemoveSpecialCharacters(CV.Name)+':'+cast(CV.Id as varchar(100)),
			1
		FROM CarModels CMO WITH(NOLOCK)
		INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CMO.ID = CV.CarModelId WHERE CV.ID = @VersionId

		INSERT INTO [ac].[SRC_Keywords] ([Name],[KeywordTypeId],[ReferenceId],[DisplayName],[IsNew],[IsUsed],[Value],[IsAutomated])
		SELECT [ac].[RSC_ExceptSpaces](CMO.Name + ' ' + CV.Name),
			6,
			CV.ID,
			CMA.Name + ' ' + CMO.Name + ' ' + CV.Name DN,
			CV.New,
			CV.Used,
			ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100))+'|'+CMO.MaskingName+':'+cast(CMO.Id as varchar(100))
			+'|'+ac.RemoveSpecialCharacters(CV.Name)+':'+cast(CV.Id as varchar(100)),
			1
		FROM CarModels CMO WITH(NOLOCK)
		INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
		INNER JOIN CarVersions CV WITH(NOLOCK) ON CMO.ID = CV.CarModelId WHERE CV.ID = @VersionId
	END

	ELSE IF(@Action = 2)
	BEGIN
		UPDATE KW
		SET Name = [ac].[RSC_ExceptSpaces](CMA.Name + ' ' + CMO.Name + ' ' + CV.Name),
			DisplayName = CMA.Name + ' ' + CMO.Name + ' ' + CV.Name,
			IsNew = CV.New,
			IsUsed = CV.Used,
			Value = ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100))+'|'+CMO.MaskingName+':'+cast(CMO.Id as varchar(100))
			+'|'+ac.RemoveSpecialCharacters(CV.Name)+':'+cast(CV.Id as varchar(100))
		--SELECT *
		FROM AC.SRC_Keywords KW WITH(NOLOCK)
		INNER JOIN CarVersions CV WITH(NOLOCK) ON KW.ReferenceId = CV.ID
		INNER JOIN CarModels CMO WITH(NOLOCK) ON CV.CarModelId = CMO.ID
		INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
		WHERE KW.KeywordTypeId = 5 AND KW.IsAutomated = 1 and CV.ID = @VersionId

		UPDATE KW
		SET Name = [ac].[RSC_ExceptSpaces](CMO.Name + ' ' + CV.Name),
			DisplayName = CMA.Name + ' ' + CMO.Name + ' ' + CV.Name,
			IsNew = CV.New,
			IsUsed = CV.Used,
			Value = ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100))+'|'+CMO.MaskingName+':'+cast(CMO.Id as varchar(100))
			+'|'+ac.RemoveSpecialCharacters(CV.Name)+':'+cast(CV.Id as varchar(100))
		--SELECT *
		FROM AC.SRC_Keywords KW WITH(NOLOCK)
		INNER JOIN CarVersions CV WITH(NOLOCK) ON KW.ReferenceId = CV.ID
		INNER JOIN CarModels CMO WITH(NOLOCK) ON CV.CarModelId = CMO.ID
		INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
		WHERE KW.KeywordTypeId = 6 AND KW.IsAutomated = 1 and CV.ID = @VersionId
	END

	ELSE IF(@Action = 3)
	BEGIN
		DELETE FROM AC.SRC_Keywords WHERE KeywordTypeId IN (5,6) AND ReferenceId = @VersionId
	END
END
