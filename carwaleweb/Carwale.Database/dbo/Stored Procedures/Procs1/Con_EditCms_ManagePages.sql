IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_ManagePages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_ManagePages]
GO

	CREATE PROCEDURE [dbo].[Con_EditCms_ManagePages]      
(      
 @Id AS Numeric(18,0),      
 @BasicId AS Numeric(18,0),      
 @PageName As Varchar(50),      
 @Priority AS Int,      
 @IsActive AS Bit, 
 @UpdatedBy AS NUMERIC(18, 0),
 @Status AS Int Output      
)      
AS      
BEGIN      
 --Declare @MaxPriority AS INT      
 --SELECT @MaxPriority = MAX(Priority) FROM Con_EditCms_Pages WHERE BasicId = @BasicId AND IsActive = 1    
 --IF(@MaxPriority >= @Priority) BEGIN    
 --SET @Status = -3    
 --END ELSE BEGIN    
  IF(@Id = -1) BEGIN      
   IF EXISTS(
			SELECT 1 FROM Con_EditCms_Pages WITH(NOLOCK)
			 WHERE BasicId = @BasicId AND PageName = LTRIM(RTRIM(@PageName)) AND IsActive = @IsActive) 
			 BEGIN      
    SET @Status = -1    --Page Name already exist for this Basic Id      
   END ELSE IF EXISTS(SELECT 1 FROM Con_EditCms_Pages  WITH(NOLOCK)  WHERE BasicId = @BasicId AND Priority = @Priority) BEGIN   
 SET @Status = -3    --Page No. already exists  
   END ELSE BEGIN      
    INSERT INTO Con_EditCms_Pages (BasicId, PageName, Priority, LastUpdatedTime, LastUpdatedBy ) VALUES (@BasicId, LTRIM(RTRIM(@PageName)), @Priority, GETDATE(), @UpdatedBy)      
          
    SET @Status = 0      
   END         
  END ELSE BEGIN      
   IF NOT EXISTS(SELECT 1 FROM Con_EditCms_Pages WITH(NOLOCK)  WHERE Id = @Id) BEGIN      
    SET @Status = -2    --Record does not exist    
   END ELSE IF EXISTS(SELECT 1 FROM Con_EditCms_Pages WITH(NOLOCK)  WHERE BasicId = @BasicId AND Priority = @Priority AND ID!=@Id) BEGIN   
 SET @Status = -3    --Page No. already exists    
   END ELSE BEGIN      
    UPDATE Con_EditCms_Pages SET PageName = LTRIM(RTRIM(@PageName)), Priority = @Priority, IsActive = @IsActive , LastUpdatedTime = GETDATE(), LastUpdatedBy = @UpdatedBy     
    WHERE Id = @Id      
          
    SET @Status = 0      
   END      
  END      
 --END    
END      
      
 -- drop proc Con_EditCms_ManagePages    
--DECLARE @a int      
--EXEC Con_EditCms_ManagePages 'U',1,'Intro1',1, '1', @a  output      
--Select @a      
      
      
--select * from Con_EditCms_Pages      
      
--select * from Con_EditCms_Basic      
      
--SELECT Id, BasicId, PageName, Priority -      
--FROM Con_EditCms_Pages       
--WHERE ID = 2      
--AND IsActive = 1
