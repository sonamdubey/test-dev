IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_QueueRulesSaveData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_QueueRulesSaveData]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 15 Jan 2014
-- Description:	To insert data in CRM_ADM_QueueRuleParams
-- =============================================
CREATE PROCEDURE [dbo].[CRM_QueueRulesSaveData]
@QueueId		INT,
@MakeId			INT,
@ModelId		VARCHAR(MAX),
@CityId			VARCHAR(MAX),
@UpdatedOn		DATETIME,
@UpdatedBy		NUMERIC,
@Source			VARCHAR(MAX),
@DealerId		INT

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
						INSERT INTO CRM_ADM_QueueRuleParams(QueueId, MakeId,ModelId, CityId,UpdatedOn, UpdatedBy,SourceId,DealerId)
						SELECT @QueueId, @MakeId,ListMember,@StrCity,@UpdatedOn, @UpdatedBy,@StrSource,@DealerId FROM fnSplitCSV(@modelId)
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
						INSERT INTO CRM_ADM_QueueRuleParams(QueueId, MakeId,ModelId, CityId,UpdatedOn, UpdatedBy,SourceId,DealerId)
						SELECT @QueueId, @MakeId,ListMember,@StrCity,@UpdatedOn, @UpdatedBy,@StrSource,@DealerId FROM fnSplitCSV(@modelId)
					END
				END

		END
END



