IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FLCRulesSaveData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FLCRulesSaveData]
GO

	CREATE PROCEDURE [dbo].[CRM_FLCRulesSaveData]
@QueueId		INT,
@MakeId			INT,
@ModelId		VARCHAR(MAX),
@CityId			VARCHAR(MAX),
@Source			VARCHAR(MAX),
@UpdatedBy		NUMERIC,
@Rank			SMALLINT

AS 

BEGIN
	DECLARE @CityIndx SMALLINT,@StrCity VARCHAR(MAX)
	DECLARE @SourceIndx SMALLINT,@StrSource VARCHAR(MAX) 

	WHILE @CityId <> ''
		BEGIN
			SET @CityIndx =	CHARINDEX(',',@CityId)
			DECLARE @SourceTemp VARCHAR(MAX) = @Source

			IF @CityIndx > 0 
				BEGIN
					SET @StrCity = LEFT(@CityId,@CityIndx-1)
					
					WHILE @SourceTemp <> ''
					BEGIN
						SET @SourceIndx = CHARINDEX(',',@SourceTemp)
						IF @SourceIndx > 0
							BEGIN
								SET @StrSource = LEFT(@SourceTemp,@SourceIndx-1)
								SET @SourceTemp = RIGHT(@SourceTemp,LEN(@SourceTemp)-@SourceIndx)
							END
						ELSE
							BEGIN
								SET @StrSource = @SourceTemp
								SET @SourceTemp = ''
							END
						INSERT INTO CRM_ADM_FLCRules(GroupId,MakeId,ModelId,CityId,SourceId, UpdatedBy,Rank) 
						SELECT @QueueId, @MakeId, ListMember ,@StrCity, @StrSource, @UpdatedBy, @Rank FROM fnSplitCSV(@modelId)
					END

					SET @CityId = RIGHT(@CityId,LEN(@CityId)-@CityIndx)
				END
			ELSE
				BEGIN
					SET @StrCity = @CityId
					SET @CityId = ''
					WHILE @SourceTemp <> ''
					BEGIN
						SET @SourceIndx = CHARINDEX(',',@SourceTemp)
						IF @SourceIndx > 0
							BEGIN
								SET @StrSource = LEFT(@SourceTemp,@SourceIndx-1)
								SET @SourceTemp = RIGHT(@SourceTemp,LEN(@SourceTemp)-@SourceIndx)
							END
						ELSE
							BEGIN
								SET @StrSource = @SourceTemp
								SET @SourceTemp = ''
							END
						INSERT INTO CRM_ADM_FLCRules(GroupId,MakeId,ModelId,CityId,SourceId, UpdatedBy,Rank) 
						SELECT @QueueId, @MakeId, ListMember ,@StrCity, @StrSource, @UpdatedBy, @Rank FROM fnSplitCSV(@modelId)
					END
				END

		END
END

