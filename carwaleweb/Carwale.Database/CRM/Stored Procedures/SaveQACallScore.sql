IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[SaveQACallScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[SaveQACallScore]
GO

	-- =============================================
-- Author:		<Amit Kumar>
-- Create date: <26th july 2012>
-- Description:	<This procedure seprates string based on delimeter '|' and then again separate them based on delimeter ':'>
-- =============================================
CREATE PROCEDURE [CRM].[SaveQACallScore]
@String NVARCHAR(1000),
@QACallDataId NUMERIC(18,0)

AS
BEGIN
		DECLARE @NextString VARCHAR(40)
		DECLARE @Pos INT
		DECLARE @NextPos INT
		DECLARE @Delimiter VARCHAR(40)

		DECLARE @NextSubString VARCHAR(40)
		DECLARE @SubPos INT
		DECLARE @NextSubPos INT
		DECLARE @SubString VARCHAR(1000)
		DECLARE @SubDelimiter VARCHAR(40)

		DECLARE @SubHeadId NUMERIC(18,0)
		DECLARE @Score FLOAT

		--SET @String ='as:123|bfgh:21|c:3|drrtyy:4ffff'
		SET @Delimiter = '|'
		SET @String = @String + @Delimiter
		SET @Pos = charindex(@Delimiter,@String)
		--SET @NextSubString = 'error'

		WHILE (@pos <> 0)
			BEGIN
				SET @NextString = substring(@String,1,@Pos - 1)
					SET @SubString =@NextString
					SET @SubDelimiter = ':'
					SET @SubString = @SubString + @SubDelimiter
					SET @SubPos = charindex(@SubDelimiter,@SubString)
					WHILE(@SubPos<>0)
						BEGIN
							SET @NextSubString=substring(@SubString,1,@SubPos - 1)
							SET @SubHeadId= CAST(@NextSubString AS NUMERIC(18,0))
							SET @SubString = substring(@SubString,@SubPos+1,len(@SubString))
							SET @SubPos = charindex(@SubDelimiter,@SubString)
							
							SET @NextSubString=substring(@SubString,1,@SubPos - 1)
							--PRINT @NextSubString 
							SET @Score = CASE WHEN @NextSubString = '-1' THEN null WHEN @NextSubString != '-1' THEN CAST(@NextSubString AS FLOAT) END
							SET @SubString = substring(@SubString,@SubPos+1,len(@SubString))
							SET @SubPos = charindex(@SubDelimiter,@SubString)
							
							--SELECT @SubHead AS SubHeadId , @Score AS Score, @QACallDataId AS QACallDataId
							UPDATE CRM.QACallScore SET Score=@Score WHERE QACallDataId=@QACallDataId
							AND SubheadId=@SubHeadId
							IF @@ROWCOUNT=0
								INSERT INTO CRM.QACallScore (QACallDataId,SubheadId,Score) 
								VALUES (@QACallDataId,@SubHeadId,@Score)
						END
				SET @String = substring(@String,@pos+1,len(@String))
				SET @pos = charindex(@Delimiter,@String)
			END 
	
END	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	