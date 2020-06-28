<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupUploadfile.aspx.cs" Inherits="APKOnline.popupUploadfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="row">  
   <asp:FileUpload runat="server" ID="UploadImages" AllowMultiple="true" />  
     <asp:Button runat="server" ID="uploadedFile" Text="Upload" OnClick="uploadFile_Click" />  

   </div> 

        <div class="row">     
               <asp:Label ID="Label1" runat="server" Text="file ที่ upload แล้ว" />  
   <asp:Label ID="listofuploadedfiles" runat="server" />  
</div> 

    </form> 

</body>
</html>
  