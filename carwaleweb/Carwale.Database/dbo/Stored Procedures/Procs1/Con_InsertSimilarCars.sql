IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_InsertSimilarCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_InsertSimilarCars]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <19/3/2014>
-- Description:	<Insert cars having similar bodystyle and subsegment >
-- Modified By : Vinay Kumar Prajapati 23 May 2014 For Order Model List on the basis of PQCount(Price Quote) From Table "TopVersionCar"
-- =============================================
CREATE PROCEDURE [dbo].[Con_InsertSimilarCars]
	@ModelId	BIGINT,
	@BodyStyleId	INT,
	@SubSegmentId	INT
AS
BEGIN

	--DECLARE @ROWCNT			INT
	DECLARE @ModelList	VARCHAR(MAX)
	----FIND BODYSTYLE  AND SUBSEGMENTID OF MODEL
	--SELECT @BodyStyleId=CM.ModelBodyStyle,@SubSegmentId=CM.SubSegmentID
	--FROM CarModels CM WITH(NOLOCK)
	--WHERE CM.ID=@ModelId

	--FIND OTHER MODELS HAVING SIMILAR BODYSTYLE AND SUBSEGMENTID
	------------------------------------------------------------------------------
	SELECT  @ModelList = COALESCE(@ModelList + ', ', '') + CAST(CM.Id AS varchar(5)) 
    FROM CarModels AS CM WITH(NOLOCK)
	INNER JOIN TopVersionCar AS TVC WITH(NOLOCK) ON CM.ID=TVC.Modelid
	WHERE CM.ModelBodyStyle=@BodyStyleId AND CM.SubSegmentID = @SubSegmentId AND CM.New=1 AND CM.IsDeleted = 0	AND CM.Id <> @ModelId    
	ORDER BY TVC.PQCount DESC


	IF @ModelList <> '' AND @ModelList IS NOT NULL
		BEGIN
			--INSERT SIMILAR MODELS
			UPDATE SimilarCarModels 
			SET SimilarModels=@ModelList , UpdatedOn = GETDATE() , IsActive = 1
			WHERE ModelId = @ModelId
			
			IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO SimilarCarModels (ModelId,SimilarModels,UpdatedOn,IsActive)
					VALUES (@ModelId,@ModelList,GETDATE(),1)		
				END
		END

END
