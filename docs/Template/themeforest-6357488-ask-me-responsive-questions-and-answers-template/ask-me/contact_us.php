<?php
function clean_text($text='') {
	$text = trim($text);
	$text = strip_tags($text);
	$text = addslashes($text);
	$text = htmlspecialchars($text);
	return $text;
}
if (!$_POST) {
	die();
}else {
	if (empty($_POST["name"]) && empty($_POST["mail"]) && empty($_POST["message"])) {
		echo "all_empty";
	}else if (empty($_POST["name"])) {
		echo "empty_name";
	}else if (empty($_POST["mail"])) {
		echo "empty_mail";
	}else if (empty($_POST["message"])) {
		echo "empty_message";
	}else {
		// edit this only :)
		$your_email = "youremail@gmail.com";
		$your_name = "vbegy";
		
		$name	 = clean_text($_POST["name"]);
		$mail	 = clean_text($_POST["mail"]);
		$url	 = clean_text($_POST["url"]);
		$message = clean_text($_POST["message"]);
		
		$headers  = "From: $name <$mail>\n";
		$headers  = "To: ".$your_name." <".$your_email.">\n";
		$headers .= 'Content-type: text/html; charset=UTF-8'. "\r\n";
		$headers .= "Reply-To: $mail\n";
		$msg	  = "New Message\n<br />";
		$msg	 .= "Name : \t$name\r\n<br />";
		$msg	 .= "Email : \t$mail\r\n<br />";
		if (isset($url)) {
			$msg	 .= "URL : \t$url\r\n<br />";
		}
		$msg	 .= "Message : <br />\t$message\r\n<br /><br />";
		$subject  = "New Message\n"; 
		echo "done";
		$done = @mail($your_email,$subject,$msg,$headers);
	}
}
?>