IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DO_InsertMakeModelforOffer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DO_InsertMakeModelforOffer]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 24th Nov 2014
-- Description:	to add models and versions against offer
-- =============================================
CREATE PROCEDURE [dbo].[DO_InsertMakeModelforOffer]
	@OfferId	INT,
	@MakeId		INT,
	@ModelId	VARCHAR(250),
	@VersionId	VARCHAR(250),
	@EnteredBy	INT
AS
BEGIN
	SELECT OfferId FROM DealerOffersVersion WHERE OfferId = @OfferId

	IF @@ROWCOUNT > 0
	BEGIN
		DELETE FROM DealerOffersVersion WHERE OfferId = @OfferId
	END

	DECLARE @TempTblModel TABLE
	(
		ID INT IDENTITY(1,1),
		Model INT
	)
	DECLARE @TempModel		INT,
			@ModelCounter	INT,
			@i				TINYINT = 1

	INSERT INTO @TempTblModel(Model) SELECT ListMember FROM fnSplitCSV(@ModelId) 
		
	SELECT @ModelCounter = COUNT(ID) FROM @TempTblModel
	
	WHILE @ModelCounter > 0
	BEGIN
		SELECT @TempModel = Model FROM @TempTblModel WHERE ID = @i
		SET @i = @i + 1
		SET @ModelCounter = @ModelCounter - 1
		
		SELECT ListMember FROM fnSplitCSV(@VersionId) WHERE ListMember = -1

		IF @@ROWCOUNT > 0
		BEGIN
			INSERT INTO DealerOffersVersion(OfferId,VersionId,ModelId,MakeId)
			SELECT @OfferId,ListMember,@TempModel,@MakeId FROM fnSplitCSV(@VersionId)
		END
		ELSE
		BEGIN
			INSERT INTO DealerOffersVersion(OfferId,VersionId,ModelId,MakeId)
			SELECT @OfferId,ID,@TempModel,@MakeId FROM CarVersions WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@VersionId)) AND CarModelId = @TempModel 
		END	
	END
	
	DECLARE @IsActive BIT
	SELECT @IsActive = IsActive FROM DealerOffers WHERE ID = @OfferId

	IF @IsActive = 1
	BEGIN
		EXEC DO_AP_DeleteInActiveModelOffers
	END
	
	INSERT INTO DealerOffersVersionLog (OfferId,MakeId,ModelId,VersionId,EnteredBy,EntryDate)
	SELECT OfferId,MakeId,ModelId,VersionId,@EnteredBy,GETDATE() FROM DealerOffersVersion
END
