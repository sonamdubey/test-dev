IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[InsertCategory_Hierarchy]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[InsertCategory_Hierarchy]
GO

	CREATE PROCEDURE [CD].[InsertCategory_Hierarchy]
@CategoryName  VARCHAR(100),         
@CategoryID NUMERIC,
@Status NUMERIC OUTPUT  
AS      
       
BEGIN 
		DECLARE
			@hid     AS HIERARCHYID,
			@mgr_hid  AS HIERARCHYID,
			@last_child_hid AS HIERARCHYID;
 
 

			IF @CategoryID is NULL
			begin
				SET @hid = HIERARCHYID::GetRoot();
				end
			ELSE
				BEGIN
						SET @mgr_hid = (SELECT HierId FROM CD.CategoryMaster WITH (UPDLOCK) WHERE CategoryMasterID = @CategoryID);
						SET @last_child_hid =(SELECT MAX(HierId) FROM CD.CategoryMaster WHERE HierId.GetAncestor(1) = @mgr_hid);
						SET @hid = @mgr_hid.GetDescendant(@last_child_hid, NULL);
				END
    
    
  
	   IF EXISTS (SELECT CategoryMasterID FROM CD.CategoryMaster WHERE CategoryName = @CategoryName)
			BEGIN
				SET @Status = 0
			END	
	   ELSE	      
		   BEGIN      
				
				INSERT INTO CD.CategoryMaster(CategoryName,HierId)      
				VALUES (@CategoryName,@hid)      
				
				SET @Status = SCOPE_IDENTITY()      
		   END      
 
END
