IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[AC].[UpdateKeywordsByMakeID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [AC].[UpdateKeywordsByMakeID]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [ac].[UpdateKeywordsByMakeID]
	-- Add the parameters for the stored procedure here
	@MakeId INT,
	@Action TINYINT --	1 = INSERT, 2 = UPDATE, 3 = DELETE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@Action = 1)
	BEGIN
		INSERT INTO [ac].[SRC_Keywords]
			([Name],[KeywordTypeId],[ReferenceId],[DisplayName],[Value],[IsAutomated],IsNew,IsUsed)
		SELECT [ac].[RSC_ExceptSpaces](CMA.Name),
			1,
			CMA.ID,
			CMA.Name,
			ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100)),
			1,
			CMA.New,
			CMA.Used
		FROM CarMakes CMA WITH(NOLOCK) WHERE CMA.ID = @MakeId
	END

	ELSE IF(@Action = 2)
	BEGIN
		UPDATE KW
		SET Name = [ac].[RSC_ExceptSpaces](CMA.Name ),
			DisplayName = CMA.Name ,
			Value = ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100)),
			IsNew = CMA.New,
			IsUsed = CMA.Used
		FROM AC.SRC_Keywords KW WITH(NOLOCK)
		INNER JOIN CarMakes CMA WITH(NOLOCK) ON KW.ReferenceId = CMA.ID
		WHERE KW.KeywordTypeId = 1 AND KW.IsAutomated = 1 AND CMA.ID = @MakeId

		UPDATE KW
		SET Name = [ac].[RSC_ExceptSpaces](CMA.Name + ' ' + CMO.Name),
			DisplayName = CMA.Name + ' ' + CMO.Name,
			IsNew = CMO.New,
			IsUsed = CMO.Used,
			Value = ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100))+'|'+CMO.MaskingName+':'+cast(CMO.Id as varchar(100))
		--SELECT *
		FROM AC.SRC_Keywords KW WITH(NOLOCK)
		INNER JOIN CarModels CMO WITH(NOLOCK) ON KW.ReferenceId = CMO.ID
		INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
		WHERE KW.KeywordTypeId = 4 AND KW.IsAutomated = 1 AND CMA.ID = @MakeId AND CMO.IsDeleted = 0

		UPDATE KW
		SET Name = [ac].[RSC_ExceptSpaces](CMO.Name),
			DisplayName = CMA.Name + ' ' + CMO.Name,
			IsNew = CMO.New,
			IsUsed = CMO.Used,
			Value = ac.RemoveSpecialCharacters(CMA.Name)+':'+cast(CMA.Id as varchar(100))+'|'+CMO.MaskingName+':'+cast(CMO.Id as varchar(100))
		--SELECT *
		FROM AC.SRC_Keywords KW WITH(NOLOCK)
		INNER JOIN CarModels CMO WITH(NOLOCK) ON KW.ReferenceId = CMO.ID
		INNER JOIN CarMakes CMA WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
		WHERE KW.KeywordTypeId = 2 AND KW.IsAutomated = 1 AND CMA.ID = @MakeId AND CMO.IsDeleted = 0

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
		WHERE KW.KeywordTypeId = 5 AND KW.IsAutomated = 1 AND CMA.ID = @MakeId AND CMO.IsDeleted = 0 AND CV.IsDeleted = 0

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
		WHERE KW.KeywordTypeId = 6 AND KW.IsAutomated = 1 AND CMA.ID = @MakeId AND CMO.IsDeleted = 0 AND CV.IsDeleted = 0
	END

	ELSE IF(@Action = 3)
	BEGIN
		DELETE FROM AC.SRC_Keywords WHERE KeywordTypeId IN (1) AND ReferenceId = @MakeId
		DELETE FROM AC.SRC_Keywords WHERE KeywordTypeId IN (2,4)
		AND ReferenceId IN (SELECT ID FROM CarModels WITH(NOLOCK) WHERE CarMakeId = @MakeId)
		DELETE FROM AC.SRC_Keywords WHERE KeywordTypeId IN (5,6)
		AND ReferenceId IN 
			(SELECT CV.ID FROM CarModels CM WITH(NOLOCK) 
			INNER JOIN CarVersions CV WITH(NOLOCK) ON CM.ID = CV.CarModelId
			WHERE CM.CarMakeId = @MakeId)
	END
END
