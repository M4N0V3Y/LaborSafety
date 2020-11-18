/*
<!-- HTML Part -->
<input id="file" type="file" accept="image/*">
<script>
    document.getElementById("file").addEventListener("change", function (event) {
	compress(event);
});
</script>
*/

function compressImage(event, maxSizePerm) {
  const width = 0;
  const height = 0;
  const fileName = event.target.name;

    const img = new Image();
    img.src = event.target.result;
    img.onload = () => {
        const elem = document.createElement('canvas');

        width = img.width;
        height = img.height;

        if (width > maxSizePerm) {
            const percenSub = (100 * maxSizePerm) / width;

            width = maxSizePerm;
            height = elem.height - ((elem.height * percenSub) / 100);
        }

        if (height > maxSizePerm) {
            const percenSub = (100 * maxSizePerm) / height;

            width = width - ((width * percenSub) / 100);
            height = maxSizePerm;
        }

        elem.width = width;
        elem.height = height;

        const ctx = elem.getContext('2d');
        // img.width and img.height will contain the original dimensions
        ctx.drawImage(img, 0, 0, width, height);
        ctx.canvas.toBlob((blob) => {
          const file = new File([blob], fileName, {
            type: 'image/jpeg',
            lastModified: Date.now()
          });
        }, 'image/jpeg', 1);
    }
}
