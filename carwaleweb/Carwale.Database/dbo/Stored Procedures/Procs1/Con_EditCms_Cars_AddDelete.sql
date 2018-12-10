IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Cars_AddDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Cars_AddDelete]
GO

	CREATE PROCEDURE [dbo].[Con_EditCms_Cars_AddDelete]  
(  
 @Id AS NUMERIC(18, 0) = -1,  
 @BasicId AS NUMERIC(18, 0) = -1,  
 @MakeId AS NUMERIC(18, 0) = -1,  
 @ModelId AS NUMERIC(18, 0) = -1,  
 @VersionId AS NUMERIC(18, 0) = -1,
 @UpdatedBy AS NUMERIC(18, 0)
)  
AS  
BEGIN  
 IF(@Id = -1) BEGIN  
  UPDATE Con_EditCms_Cars SET IsActive = 1, LastUpdatedTime = GETDATE(), LastUpdatedBy = @UpdatedBy WHERE BasicId = @BasicId AND MakeId = @MakeId AND ModelId = @ModelId AND VersionId = @VersionId  
  IF(@@ROWCOUNT = 0) BEGIN  
   INSERT INTO Con_EditCms_Cars (BasicId, MakeId, ModelId, VersionId, LastUpdatedTime, LastUpdatedBy) VALUES (@BasicId, @MakeId, @ModelId, @VersionId, GETDATE(), @UpdatedBy)  
  END  
 END ELSE BEGIN  
  UPDATE Con_EditCms_Cars SET IsActive = 0, LastUpdatedTime = GETDATE(), LastUpdatedBy = @UpdatedBy WHERE Id = @Id  
 END  
END



--drop Procedure Con_EditCms_Cars_AddDelete