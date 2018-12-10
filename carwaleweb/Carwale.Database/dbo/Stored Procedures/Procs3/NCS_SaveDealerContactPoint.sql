IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_SaveDealerContactPoint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_SaveDealerContactPoint]
GO

	
-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 30th December 2013
-- Description : Save details  into NCS_DealerContactPoint 
-- Modifier	   : Ruchira Patil on 18th June 2014 -- Logging the updated contact points and also added updated by and updated on field in NCS_DealerContactPoint
-- =============================================

CREATE  PROCEDURE [dbo].[NCS_SaveDealerContactPoint]
    (
	@Id                INT,
	@DealerId          INT ,
	@MakeId            VARCHAR(18),
	@ModelId           VARCHAR(18),
	@ContactName       VARCHAR(30),
	@Designation       VARCHAR(50),
	@Email             VARCHAR(200),
	@Mobile            VARCHAR(50),
	@AltMobNumber      VARCHAR(15),
	@ContactPointType  INT,
	@UpdatedBy		   INT,
	@Status			   BIT OUTPUT
	)
 AS
   BEGIN
	   IF @Id = -1  --for inserting data 
		   BEGIN
			  SELECT TOP 1 DCP.Id FROM NCS_DealerContactPoint AS DCP WITH(NOLOCK)
			  WHERE DCP.DealerId=@DealerId AND  DCP.MakeId=@MakeId AND DCP.ModelId=@ModelId AND DCP.ContactName=@ContactName
			  AND  DCP.Designation= @Designation AND DCP.ContactPointType=@ContactPointType

			   IF @@ROWCOUNT = 0
				   BEGIN 
					   INSERT INTO NCS_DealerContactPoint
					   (DealerId,ModelId,MakeId,ContactName,Designation,Email,Mobile,AlternateMobile,ContactPointType,UpdatedBy,UpdatedOn)      
					   VALUES(@DealerId,@ModelId,@MakeId,@ContactName,@Designation,@Email,@Mobile,@AltMobNumber,@ContactPointType,@UpdatedBy,GETDATE())
						SET  @Status = 1    
				   END  
			   ELSE
					BEGIN 
					   SET  @Status = 0    
					END    
			
			 END 
		ELSE	     --Upadate details  
			BEGIN
			  SELECT TOP 1 DCP.Id FROM NCS_DealerContactPoint AS DCP WITH(NOLOCK)
			  WHERE DCP.DealerId=@DealerId AND  DCP.MakeId=@MakeId AND DCP.ModelId=@ModelId AND DCP.ContactName=@ContactName
			  AND  DCP.Designation= @Designation AND DCP.ContactPointType=@ContactPointType AND DCP.Id <> @Id
			 
			  IF @@ROWCOUNT = 0
				   BEGIN 
					 UPDATE NCS_DealerContactPoint
					 SET  DealerId= @DealerId,ModelId=@ModelId,MakeId=@MakeId,
					 ContactName=@ContactName,Designation=@Designation,
                     Email=@Email,Mobile=@Mobile,AlternateMobile=@AltMobNumber,ContactPointType=@ContactPointType 
					 WHERE Id=@Id    

					 INSERT INTO NCS_DealerContactPointLog
					 (ContactPtId,DealerId,ModelId,MakeId,ContactName,Designation,Email,Mobile,AlternateMobile,ContactPointType,UpdatedBy,UpdatedOn)      
					 VALUES(@Id,@DealerId,@ModelId,@MakeId,@ContactName,@Designation,@Email,@Mobile,@AltMobNumber,@ContactPointType,@UpdatedBy,GETDATE())

					 SET  @Status = 1    
				   END  
			   ELSE
					BEGIN 
					   SET  @Status = 0    
					END   

			END 		      
    END
    
   