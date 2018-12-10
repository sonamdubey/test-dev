IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MH59_UpdateSpareCost]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MH59_UpdateSpareCost]
GO

	
CREATE PROCEDURE [dbo].[MH59_UpdateSpareCost]

	@sIds VARCHAR(8000), -- List of delimited items
	@sTotCost VARCHAR(8000), -- List of delimited items
	@sLabCharges VARCHAR(8000), -- List of delimited items
	@sDelimiter VARCHAR(8000) = ',', -- delimiter that separates items
 
	@Status	INT OUTPUT
				
 AS
	
BEGIN
	DECLARE @Id VARCHAR(8000)
	DECLARE @Cost VARCHAR(8000)
	DECLARE @LCharge VARCHAR(8000)
	
	WHILE CHARINDEX(@sDelimiter,@sIds,0) <> 0
	BEGIN 
		 SELECT
		  @Id=RTRIM(LTRIM(SUBSTRING(@sIds,1,CHARINDEX(@sDelimiter,@sIds,0)-1))),
		  @sIds=RTRIM(LTRIM(SUBSTRING(@sIds,CHARINDEX(@sDelimiter,@sIds,0)+LEN(@sDelimiter),LEN(@sIds)))),
		  
		  @Cost=RTRIM(LTRIM(SUBSTRING(@sTotCost,1,CHARINDEX(@sDelimiter,@sTotCost,0)-1))),
		  @sTotCost=RTRIM(LTRIM(SUBSTRING(@sTotCost,CHARINDEX(@sDelimiter,@sTotCost,0)+LEN(@sDelimiter),LEN(@sTotCost)))),
		  
		  @LCharge=RTRIM(LTRIM(SUBSTRING(@sLabCharges,1,CHARINDEX(@sDelimiter,@sLabCharges,0)-1))),
		  @sLabCharges=RTRIM(LTRIM(SUBSTRING(@sLabCharges,CHARINDEX(@sDelimiter,@sLabCharges,0)+LEN(@sDelimiter),LEN(@sLabCharges))))

		  IF LEN(@Id) > 0
			BEGIN
				UPDATE Con_SpareCost SET TotalCost = @Cost, LabourCharges = @LCharge WHERE ID = @Id
				SET @Status = 1
			END	
	END	
		
END
