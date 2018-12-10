IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SCSaveServiceDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SCSaveServiceDetails]
GO

	

CREATE PROCEDURE [dbo].[OLM_SCSaveServiceDetails]
(
	@PartId		NUMERIC(18,0),
	@VersionId	INT,
	@PartCost INT,
	@LabourPercentege INT,
	@price		INT ,
	@Quantity   VARCHAR(10),
	@Type       int,--Defined for Service or Replacement Type=1 for Service & Labour Cost, Type=2 for Replacement
	@15K		INT ,
	@30K		INT,
	@45K		INT,
	@60K		INT,
	@75K		INT,
	@90K		INT,
	@105K		INT,
	@120K		INT,
	@135K		INT,
	@150K	    INT   
	--@Status     BIT OUTPUT
	)
	
 AS
		BEGIN
		  IF @Type=1
		     BEGIN
					SELECT VersionId FROM OLM_SCServiceParts WITH (NOLOCK) WHERE VersionId = @VersionId AND PartId=@PartId
					IF @@ROWCOUNT = 0
						BEGIN

							INSERT INTO OLM_SCServiceParts(PartId, VersionId,Price,Quantity,[15K],[30K],[45K],[60K],[75K],[90K],[105K],[120K],[135K],[150K])			
							Values(@PartId, @VersionId, @price, @Quantity,@15K,@30K,@45K,@60K,@75K,@90K,@105K,@120K,@135K,@150K)	
					  		
						END
					 ELSE
						BEGIN
						
						UPDATE OLM_SCServiceParts SET Price= @Price,Quantity= @Quantity ,
						[15K]=@15K,[30K]=@30K,[45K]=@45K,[60K]=@60K,[75K]=@75K,[90K]=@90K,[105K]=@105K,[120K]=@120K,[135K]=@135K,[150K]=@150K
						WHERE PartId = @PartId AND VersionId=@VersionId
								
						END
		       END
		     ELSE IF @Type=2 
		      BEGIN
				
					SELECT VersionId FROM OLM_SCReplacementParts WITH (NOLOCK) WHERE VersionId = @VersionId AND PartId=@PartId
					IF @@ROWCOUNT = 0
						BEGIN
							
							INSERT INTO OLM_SCReplacementParts(PartId, VersionId,PartCost,LabourPercentage)			
							Values(@PartId, @VersionId,@PartCost,@LabourPercentege )	
					  		
						END
					 ELSE
						BEGIN
				        
						UPDATE OLM_SCReplacementParts SET PartCost= @PartCost,LabourPercentage= @LabourPercentege
						WHERE PartId = @PartId AND VersionId=@VersionId
								
						END
		       END
		        		
		END
	

